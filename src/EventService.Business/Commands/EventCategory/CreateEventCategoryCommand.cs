using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Business.Commands.EventCategory.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.EventService.Validation.EventCategory.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.EventCategory;

public class CreateEventCategoryCommand: ICreateEventCategoryCommand
{
  private readonly IAccessValidator _accessValidator;
  private readonly IEventCategoryRepository _repository;
  private readonly IDbEventCategoryMapper _mapper;
  private readonly ICreateEventCategoryRequestValidator _validator;
  private readonly IResponseCreator _responseCreator;
  private readonly IHttpContextAccessor _contextAccessor;

  public CreateEventCategoryCommand(
    IAccessValidator accessValidator,
    IEventCategoryRepository repository,
    IDbEventCategoryMapper mapper,
    ICreateEventCategoryRequestValidator validator,
    IResponseCreator responseCreator,
    IHttpContextAccessor contextAccessor)
  {
    _accessValidator = accessValidator;
    _repository = repository;
    _mapper = mapper;
    _validator = validator;
    _responseCreator = responseCreator;
    _contextAccessor = contextAccessor;
  }
  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateEventCategoryRequest request)
  {
    if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveCompanyData))
    {
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden);
    }

    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(vf => vf.ErrorMessage).ToList());
    }

    DbEventCategory dbEventCategory = _mapper.Map(request);
    await _repository.CreateAsync(dbEventCategory);
    _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

    return new OperationResultResponse<Guid?>(body: dbEventCategory.Id);
  }
}
