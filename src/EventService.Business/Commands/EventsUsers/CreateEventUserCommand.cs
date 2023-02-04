using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Business.Commands.EventsUsers.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.EventsUsers;

public class CreateEventUserCommand : ICreateEventUserCommand
{
  private readonly IAccessValidator _accessValidator;
  private readonly IEventUserRepository _repository;
  private readonly IDbEventUserMapper _mapper;
  private readonly ICreateEventUserRequestValidator _validator;
  private readonly IResponseCreator _responseCreator;
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IEventRepository _eventRepository;

  public CreateEventUserCommand(
    IAccessValidator accessValidator,
    IEventUserRepository repository,
    IDbEventUserMapper mapper,
    ICreateEventUserRequestValidator validator,
    IResponseCreator responseCreator,
    IHttpContextAccessor contextAccessor,
    IEventRepository eventRepository)
  {
    _accessValidator = accessValidator;
    _repository = repository;
    _mapper = mapper;
    _validator = validator;
    _responseCreator = responseCreator;
    _contextAccessor = contextAccessor;
    _eventRepository = eventRepository;
  }
  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateEventUserRequest request)
  {
    if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers))
    {
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden, 
        new List<string>() {"you haven't rights to add users to event"});
    }

    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (request.UserId == _contextAccessor.HttpContext.GetUserId() &&
        (await _eventRepository.GetAsync(request.EventId)).Access == AccessType.Closed &&
        !await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers))
    {
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden,
        new List<string>() { "you can't add yourself to closed event" });
    }

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(er => er.ErrorMessage).ToList());
    }

    OperationResultResponse<Guid?> response = new();

    response.Body = await _repository.CreateAsync(_mapper.Map(request));

    _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

    return response;
  }
}

