using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.EventCategory.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventCategory;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class EventCategoryController : ControllerBase
  {
    [HttpDelete("remove")]
    public async Task<OperationResultResponse<bool>> DeleteAsync(
    [FromServices] IRemoveEventCategoryCommand command,
    [FromBody] RemoveEventCategoryRequest request)
    {
      return await command.ExecuteAsync(request);
    }
  }
}
