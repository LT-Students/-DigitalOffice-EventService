using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Validation.Event.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Kernel.Helpers;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.Event;

public class CreateEventCommand : ICreateEventCommand
{
  private readonly IEventRepository _repository;
  private readonly ICreateEventRequestValidator _validator;
  private readonly IDbEventMapper _mapper;
  private readonly IAccessValidator _accessValidator;
  private readonly IResponseCreator _responseCreator;
  private readonly IHttpContextAccessor _contextAccessor;

  public CreateEventCommand(
    IEventRepository repository,
    ICreateEventRequestValidator validator,
    IDbEventMapper mapper,
    IAccessValidator accessValidator,
    IResponseCreator responseCreator,
    IHttpContextAccessor contextAccessor)
  {
    _repository = repository;
    _validator = validator;
    _mapper = mapper;
    _accessValidator = accessValidator;
    _responseCreator = responseCreator;
    _contextAccessor = contextAccessor;
  }

  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateEventRequest request)
  {
    if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers))
    {
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
    }

    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
      return ResponseCreatorStatic.CreateResponse<Guid?>(
        HttpStatusCode.BadRequest,
        errors: validationResult.Errors.Select(er => er.ErrorMessage).ToList());
    }

    DbEvent dbEvent = _mapper.Map(request);

    await _repository.CreateAsync(dbEvent);

    OperationResultResponse<Guid?> response = new(body: dbEvent.Id);

    _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

    return response;
  }
}
