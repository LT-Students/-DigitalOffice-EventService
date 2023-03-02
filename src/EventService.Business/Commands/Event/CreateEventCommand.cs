using System.Collections.Generic;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Validation.Event.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Models;
using Microsoft.AspNetCore.Http;
using LT.DigitalOffice.EventService.Broker.Requests.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.EventService.Models.Dto.Enums;

namespace LT.DigitalOffice.EventService.Business.Commands.Event;

public class CreateEventCommand : ICreateEventCommand
{
  private readonly IEventRepository _eventRepository;
  private readonly IEventUserRepository _eventUserRepository;
  private readonly IEventCategoryRepository _eventCategoryRepository;
  private readonly ICreateEventRequestValidator _validator;
  private readonly IDbEventMapper _eventMapper;
  private readonly IDbEventUserMapper _eventUserMapper;
  private readonly IDbEventCategoryMapper _eventCategoryMapper;
  private readonly IAccessValidator _accessValidator;
  private readonly IResponseCreator _responseCreator;
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IEmailService _emailService;
  private readonly IUserService _userService;

  public CreateEventCommand(
    IEventRepository repository,
    IEventUserRepository eventUserRepository,
    IEventCategoryRepository eventCategoryRepository,
    ICreateEventRequestValidator validator,
    IDbEventMapper eventMapper,
    IDbEventUserMapper eventUserMapper,
    IDbEventCategoryMapper eventCategoryMapper,
    IAccessValidator accessValidator,
    IResponseCreator responseCreator,
    IHttpContextAccessor contextAccessor,
    IUserService userService,
    IEmailService emailService)
  {
    _eventRepository = repository;
    _eventUserRepository = eventUserRepository;
    _eventCategoryRepository = eventCategoryRepository;
    _eventMapper = eventMapper;
    _eventUserMapper = eventUserMapper;
    _eventCategoryMapper = eventCategoryMapper;
    _validator = validator;
    _accessValidator = accessValidator;
    _responseCreator = responseCreator;
    _contextAccessor = contextAccessor;
    _userService = userService;
    _emailService = emailService;
  }

  private async Task SendInviteEmailsAsync(List<Guid> userIds, string eventName)
  {
    List<UserData> usersData = await _userService.GetUsersDataAsync(userIds);

    if (usersData is null || !usersData.Any())
    {
      return;
    }

    foreach (UserData user in usersData)
    {
      await _emailService.SendAsync(
        user.Email,
        "Invite to event",
        $"You have been invited to event {eventName}");
    }
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(CreateEventRequest request)
  {
    Guid senderId = _contextAccessor.HttpContext.GetUserId();
    bool hasAddEditRemoveUsersRights = await _accessValidator.HasRightsAsync(senderId, Rights.AddEditRemoveUsers);
    OperationResultResponse<bool> response = new();

    if (!hasAddEditRemoveUsersRights)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
    }

    if (request.Users.Distinct().Count() != request.Users.Count())
    {
      response.Errors = new List<string>() { "Some duplicate users have been removed from the list." };
      request.Users = request.Users.Distinct().ToList();
    }

    request.CategoryIds = request.CategoryIds.Distinct().ToList();

    ValidationResult validationResult = await _validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(er => er.ErrorMessage).ToList());
    };

    DbEvent dbEvent = _eventMapper.Map(request, senderId);

    if ((dbEvent.Access == AccessType.Closed && !hasAddEditRemoveUsersRights)
        || !(dbEvent.Access == AccessType.Opened
          && (hasAddEditRemoveUsersRights
            || (!hasAddEditRemoveUsersRights
              && request.Users.Count == 1
              && request.Users.Exists(x => x.UserId == senderId)))))
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
    }

    response.Body = await _eventRepository.CreateAsync(dbEvent);

    List<DbEventUser> dbEventUsers = _eventUserMapper.Map(request, senderId, dbEvent.Id);
    response.Body = await _eventUserRepository.CreateAsync(dbEventUsers) 
      && response.Body;

    await SendInviteEmailsAsync(dbEventUsers.Where(x => x.Status == EventUserStatus.Invited).Select(x => x.UserId).ToList(), dbEvent.Name);

    response.Body = await _eventCategoryRepository.CreateAsync(_eventCategoryMapper.Map(request, senderId, dbEvent.Id))
      && response.Body;

    if (!response.Body)
    {
      _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
    else
    {
      _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
    }

    return response;
  }
}
