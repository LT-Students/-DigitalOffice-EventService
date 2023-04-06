using System;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<Guid?>> CreateCategoryController(
    [FromServices] ICreateCategoryCommand command,
    [FromBody] CreateCategoryRequest request)
  {
    return await command.ExecuteAsync(request);
  }
  
  [HttpPost("find")]
  public async Task<FindResultResponse<CategoryInfo>> FindCategoryFilter(
    [FromServices] IFindCategoryCommand command,
    [FromQuery] Guid categoryId,
    [FromQuery] FindCategoriesFilter filter,
    CancellationToken cancellationToken)
  {
    return await command.ExecuteAsync(eventId: categoryId, filter: filter, cancellationToken: cancellationToken);
  }
}

