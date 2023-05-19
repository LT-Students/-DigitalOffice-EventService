using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.EventService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Image;
using LT.DigitalOffice.EventService.Validation.Image.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.Image;

public class RemoveImageCommand : IRemoveImageCommand
{
  private readonly IEventImageRepository _repository;
  private readonly IAccessValidator _accessValidator;
  private readonly IRemoveImagesRequestValidator _validator;
  private readonly IResponseCreator _responseCreator;
  private readonly IPublish _publish;

  public RemoveImageCommand(
    IEventImageRepository repository,
    IAccessValidator accessValidator,
    IRemoveImagesRequestValidator validator,
    IResponseCreator responseCreator,
    IPublish publish)
  {
    _repository = repository;
    _accessValidator = accessValidator;
    _validator = validator;
    _responseCreator = responseCreator;
    _publish = publish;
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(RemoveImageRequest request)
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
        validationResult.Errors.ConvertAll(x => x.ErrorMessage));
    }

    OperationResultResponse<bool> response = new();

    response.Body = await _repository.RemoveAsync(request.ImagesIds);

    if (response.Body)
    {
      await _publish.RemoveImagesAsync(request.ImagesIds);
    }

    return response;
  }
}
