using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Validation.Event.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Business.Commands.Event;

public class EditEventCommand : IEditEventCommand 
{
  private readonly IEditEventRequestValidator _validator;
  private readonly IEventRepository _repository;
  private readonly IPatchDbEventMapper _mapper;
  private readonly IResponseCreator _responseCreator;
  private readonly IAccessValidator _accessValidator;

  public EditEventCommand(
    IEditEventRequestValidator validator,
    IEventRepository repository,
    IPatchDbEventMapper mapper,
    IResponseCreator responseCreator,
    IAccessValidator accessValidator)
  {
    _validator = validator;
    _repository = repository;
    _mapper = mapper;
    _responseCreator = responseCreator;
    _accessValidator = accessValidator;
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(
    Guid eventId,
    JsonPatchDocument<EditEventRequest> request)
  {
    if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers))
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
    }

    ValidationResult validationResult = await _validator.ValidateAsync((eventId, request));

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest,
        validationResult.Errors.ConvertAll(er => er.ErrorMessage));
    }

    OperationResultResponse<bool> response = new(body: await _repository.EditAsync(eventId, _mapper.Map(request)));

    if (!response.Body)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
    }

    return response;
  }
}
