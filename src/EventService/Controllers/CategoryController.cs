using System;
using System.Net;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Models.Dto.Responses.Category;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Controllers
{
  
  [ApiController]
  [Route("[controller]")]
  public class CategoryController : ControllerBase
  {
    [HttpPost("create")]
    public async Task<OperationResultResponse<Guid?>> CreateCategoryController(
      [FromServices] ICreateCategoryCommand command,
      [FromBody] CreateCategoryRequest request)
    {
      var response = await command.ExecuteAsync(request);
      return response;
    }
    
    [HttpPost("find")]
    public async Task<FindCategoryResponse> FindCategoryController(
      [FromServices] IFindCategoryCommand command,
      [FromBody] FindCategoryRequest request)
    {
      var response = await command.ExecuteAsync(request);
        
      HttpContext.Response.StatusCode = response.IsSuccess ? 
        (int)HttpStatusCode.Created : 
        (int)HttpStatusCode.NotFound;

      return response;
    }
  }
}
