using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Validation.Category.Interfaces;
using LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.AccessValidatorEngine.Interfaces;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using MassTransit;
using Microsoft.AspNetCore.Http;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace LT.DigitalOffice.EventService.Business.Commands.Category;

public class CreateCategoryCommand : ICreateCategoryCommand
{ 
    private readonly IAccessValidator _accessValidator;
    private IRequestClient<CreateCategoryRequest> _request;
    private readonly ICategoryRepository _repository;
    private readonly IDbCategoryMapper _mapper;
    private ICreateCategoryValidator _validator;
    // private readonly ICreateCategoryRequestValidator _validator;
    private readonly IResponseCreator _responseCreator;
    private readonly IHttpContextAccessor _contextAccessor;

    public CreateCategoryCommand(
      IAccessValidator accessValidator,
      ICreateCategoryValidator validator,
      IDbCategoryMapper mapper,
      ICategoryRepository repository,
      IRequestClient<CreateCategoryRequest> request,
      IHttpContextAccessor contextAccessor,
      IResponseCreator responseCreator)
    {
        _accessValidator = accessValidator;
        _validator = validator;
        _request = request;
        _mapper = mapper;
        _repository = repository;
        _contextAccessor = contextAccessor;
        _responseCreator = responseCreator;
    }
    
    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateCategoryRequest request)
    {
        if (!await _accessValidator.HasRightsAsync(Rights.AddEditRemoveUsers))
        {
          return _responseCreator.CreateFailureResponse<Guid?>(HttpStatusCode.Forbidden,
            new List<string>(){"you haven't rights to add categoris to event"});
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

        _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
        return response;
    }
}
