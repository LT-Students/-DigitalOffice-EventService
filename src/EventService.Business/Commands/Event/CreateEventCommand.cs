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

namespace LT.DigitalOffice.EventService.Business.Commands.Event;

public class CreateEventCommand : ICreateEventCommand
{
  private readonly IEventRepository _repository;
  private readonly ICreateEventRequestValidator _validator;
  private readonly IDbEventMapper _mapper;
  private readonly IAccessValidator _accessValidator;
  private readonly IResponseCreator _responseCreator;
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IEmailService _emailService;
  private readonly IUserService _userService;

  public CreateEventCommand(
    IEventRepository repository,
    ICreateEventRequestValidator validator,
    IDbEventMapper mapper,
    IAccessValidator accessValidator,
    IResponseCreator responseCreator,
    IHttpContextAccessor contextAccessor,
    IUserService userService,
    IEmailService emailService)
  {
    _repository = repository;
    _validator = validator;
    _mapper = mapper;
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
    if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers))
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
    }

    ValidationResult validationResult = await _validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(er => er.ErrorMessage).ToList());
    };

    DbEvent dbEvent = _mapper.Map(request);
    OperationResultResponse<bool> response = new();
    response.Body = await _repository.CreateAsync(dbEvent);

    if (!response.Body)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
    }

    _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

    return response;
  }
}
