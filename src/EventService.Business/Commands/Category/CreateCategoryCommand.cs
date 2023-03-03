using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Validation.Category.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Business.Commands.Category;

public class CreateCategoryCommand : ICreateCategoryCommand
{
  private readonly IAccessValidator _accessValidator;
  private readonly IHttpContextAccessor _contextAccessor;
  private readonly IDbCategoryMapper _mapper;
  private readonly ICategoryRepository _repository;
  private readonly IResponseCreator _responseCreator;
  private readonly ICreateCategoryValidator _validator;

  public CreateCategoryCommand(
    IAccessValidator accessValidator,
    IHttpContextAccessor contextAccessor,
    IDbCategoryMapper mapper,
    ICategoryRepository repository,
    IResponseCreator responseCreator,
    ICreateCategoryValidator validator)
  { 
    _accessValidator = accessValidator;
    _contextAccessor = contextAccessor;
    _mapper = mapper;
    _repository = repository;
    _responseCreator = responseCreator;
    _validator = validator;
  }

  public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateCategoryRequest request)
  {
    if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers))
    { 
      return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden,
        new List<string>());
    }

    ValidationResult validationResult = await _validator.ValidateAsync(request);

    if (!validationResult.IsValid)
    {
      return _responseCreator.CreateFailureResponse<Guid?>(
        HttpStatusCode.BadRequest,
        validationResult.Errors.Select(er => er.ErrorMessage).ToList());
    }

    OperationResultResponse<Guid?> response = new();
    response.Body = await _repository.CreateAsync(_mapper.Map(request));
    
    _contextAccessor.HttpContext.Response.StatusCode =
      response.Body == null ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.Created;
    
    return response;
  }
}
