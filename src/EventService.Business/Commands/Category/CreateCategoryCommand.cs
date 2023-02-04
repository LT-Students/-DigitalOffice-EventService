using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Validation.Category.Interfaces;
using LT.DigitalOffice.Kernel.Helpers.Interfaces;
using LT.DigitalOffice.Kernel.Responses;
using MassTransit;
using Microsoft.AspNetCore.Http;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace LT.DigitalOffice.EventService.Business.Commands.Category;


public class CreateCategoryCommand : ICreateCategoryCommand
{
    private IRequestClient<CreateCategoryRequest> _request;
    private ICreateCategoryValidator _validator;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IResponseCreator _responseCreator;

    public CreateCategoryCommand(
      ICreateCategoryValidator validator, 
      IRequestClient<CreateCategoryRequest> request,
      IHttpContextAccessor contextAccessor,
      IResponseCreator responseCreator)
    {
        _validator = validator;
        _request = request;
        _contextAccessor = contextAccessor;
        _responseCreator = responseCreator;
    }

    public async Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateCategoryRequest request)
    {
        if (request == null)
        {
            var message = "Request is empty";
            var failureResponse = new OperationResultResponse<Guid?>()
            {
                Errors = new List<string>()
            };
            failureResponse.Errors.Add(message);
            return failureResponse;
        }

        ValidationResult validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return _responseCreator.CreateFailureResponse<Guid?>(
              HttpStatusCode.BadRequest,
              validationResult.Errors.Select(er => er.ErrorMessage).ToList());
        }
        
        var response = await _request.GetResponse<OperationResultResponse<Guid?>>(request);
        _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;

        return response.Message;
    }
}
