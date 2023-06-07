using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.EventService.Business.Commands.File.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using LT.DigitalOffice.EventService.Validation.File.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.File;

public class RemoveFilesCommand : IRemoveFilesCommand
{
  private readonly IFileRepository _repository;
  private readonly IAccessValidator _accessValidator;
  private readonly IResponseCreator _responseCreator;
  private readonly IPublish _publish;
  private readonly IRemoveFilesRequestValidator _validator;

  public RemoveFilesCommand(
    IFileRepository repository,
    IAccessValidator accessValidator,
    IResponseCreator responseCreator,
    IPublish publish,
    IRemoveFilesRequestValidator validator)
  {
    _repository = repository;
    _accessValidator = accessValidator;
    _responseCreator = responseCreator;
    _publish = publish;
    _validator = validator;
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(RemoveFilesRequest request)
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

    OperationResultResponse<bool> response = new(body: await _repository.RemoveAsync(request.FilesIds));

    if (response.Body)
    {
      await _publish.RemoveFilesAsync(request.FilesIds);
    }
    else
    {
      response = _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.BadRequest, response.Errors);
    }

    return response;
  }
}
