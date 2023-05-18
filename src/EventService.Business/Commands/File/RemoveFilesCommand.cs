using System.Linq;
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
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.File;

public class RemoveFilesCommand : IRemoveFilesCommand
{
  private readonly IEventFileRepository _repository;
  private readonly IAccessValidator _accessValidator;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IResponseCreator _responseCreator;
  private readonly IPublish _publish;
  private readonly IRemoveFilesRequestValidator _validator;

  public RemoveFilesCommand(
    IEventFileRepository repository,
    IAccessValidator accessValidator,
    IHttpContextAccessor httpContextAccessor,
    IResponseCreator responseCreator,
    IPublish publish,
    IRemoveFilesRequestValidator validator)
  {
    _repository = repository;
    _accessValidator = accessValidator;
    _httpContextAccessor = httpContextAccessor;
    _responseCreator = responseCreator;
    _publish = publish;
    _validator = validator;
  }

  public async Task<OperationResultResponse<bool>> ExecuteAsync(RemoveFilesRequest request)
  {
    if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveProjects))
    {
      return _responseCreator.CreateFailureResponse<bool>(HttpStatusCode.Forbidden);
    }

    ValidationResult validationResult = await _validator.ValidateAsync(request);
    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<bool>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(x => x.ErrorMessage).ToList());
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
