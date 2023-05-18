using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.EventService.Business.Commands.Image.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Image;
using LT.DigitalOffice.EventService.Validation.Image.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.FluentValidationExtensions;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.Image;

public class RemoveImageCommand : IRemoveImageCommand
{
  private readonly IEventImageRepository _repository;
  private readonly IAccessValidator _accessValidator;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IRemoveImagesRequestValidator _validator;
  private readonly IResponseCreator _responseCreator;
  private readonly IPublish _publish;

  public RemoveImageCommand(
    IEventImageRepository repository,
    IAccessValidator accessValidator,
    IHttpContextAccessor httpContextAccessor,
    IRemoveImagesRequestValidator validator,
    IResponseCreator responseCreator,
    IPublish publish)
  {
    _repository = repository;
    _accessValidator = accessValidator;
    _httpContextAccessor = httpContextAccessor;
    _validator = validator;
    _responseCreator = responseCreator;
    _publish = publish;
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(RemoveImageRequest request)
  {
    if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
    }

    if (!_validator.ValidateCustom(request, out List<string> errors))
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.BadRequest,
        errors);
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
