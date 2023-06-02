using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.EventService.Validation.Event.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using LT.DigitalOffice.Models.Broker.Enums;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Business.Commands.Event;

public class EditEventCommand : IEditEventCommand 
{
  private readonly IEditEventRequestValidator _validator;
  private readonly IEventRepository _repository;
  private readonly IPatchDbEventMapper _mapper;
  private readonly IResponseCreator _responseCreator;
  private readonly IAccessValidator _accessValidator;
  private readonly IPublish _publish;

  public EditEventCommand(
    IEditEventRequestValidator validator,
    IEventRepository repository,
    IPatchDbEventMapper mapper,
    IResponseCreator responseCreator,
    IAccessValidator accessValidator,
    IPublish publish)
  {
    _validator = validator;
    _repository = repository;
    _mapper = mapper;
    _responseCreator = responseCreator;
    _accessValidator = accessValidator;
    _publish = publish;
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

    object isActiveOperation = request.Operations.FirstOrDefault(o =>
        o.path.EndsWith(nameof(EditEventRequest.IsActive), StringComparison.OrdinalIgnoreCase))?.value;

    if (isActiveOperation is not null && bool.TryParse(isActiveOperation.ToString(), out bool isActive) && !isActive)
    {
      DbEvent dbEvent = await _repository.GetAsync(eventId);

      List<Guid> filesIds = dbEvent.Files.Select(file => file.FileId).ToList();
      List<Guid> imagesIds = dbEvent.Images.Select(image => image.ImageId).ToList();

      if (filesIds.Any())
      {
        await _publish.RemoveFilesAsync(filesIds);
      }

      if (imagesIds.Any())
      {
        await _publish.RemoveImagesAsync(imagesIds);
      }
    }

    OperationResultResponse<bool> response = new(body: await _repository.EditAsync(eventId, _mapper.Map(request)));

    if (!response.Body)
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest);
    }

    return response;
  }
}
