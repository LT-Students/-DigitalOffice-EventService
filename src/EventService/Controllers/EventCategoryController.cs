using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.EventCategory.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventCategory;

namespace LT.DigitalOffice.EventService.Controllers;

[Route("[controller]")]
[ApiController]
public class EventCategoryController : ControllerBase
{
  [HttpPost("create")]
  [HttpPost("create")]
  public async Task<OperationResultResponse<bool>> CreateAsync(
    [FromServices] ICreateEventCategoryCommand command,
    [FromBody] CreateEventCategoryRequest request)
  {
    return await command.ExecuteAsync(request);
  }

  [HttpDelete("remove")]
    public async Task<OperationResultResponse<bool>> RemoveAsync(
    [FromServices] IRemoveEventCategoryCommand command,
    [FromBody] RemoveEventCategoryRequest request)
  {
    return await command.ExecuteAsync(request);
  }
}

