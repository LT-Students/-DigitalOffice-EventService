using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Models.Dto.Responses.Category;
using LT.DigitalOffice.EventService.Validation.Category.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace LT.DigitalOffice.EventService.Business.Commands.Category;

public class FindCategoryCommand : IFindCategoryCommand
{
    private IRequestClient<FindCategoryRequest> _request;
    private IFindCategoryValidator _validator;
    private readonly IHttpContextAccessor _contextAccessor;

    public FindCategoryCommand(
      IFindCategoryValidator validator, 
      IRequestClient<FindCategoryRequest> request,
      IHttpContextAccessor contextAccessor)
    {
        _validator = validator;
        _request = request;
        _contextAccessor = contextAccessor;
    }
    
    public async Task<FindCategoryResponse> ExecuteAsync(FindCategoryRequest request)
    {
        if (request == null)
        {
            var message = "Request is empty";
            var failureResponse = new FindCategoryResponse()
            {
                IsSuccess = false,
                Errors = new List<string>()
            };
            failureResponse.Errors.Add(message);
            _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return failureResponse;
        }

        ValidationResult validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var failureResponse = new FindCategoryResponse()
            {
                IsSuccess = false,
                Errors = validationResult.Errors.Select(err => err.ErrorMessage).ToList()
            };
            _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return failureResponse;
        }

        var response = await _request.GetResponse<FindCategoryResponse>(request);
        _contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
        
        return response.Message;
    }
}
