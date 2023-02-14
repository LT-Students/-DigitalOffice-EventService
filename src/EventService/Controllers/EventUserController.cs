using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.EventUser.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Controllers;

[Route("[controller]")]
[ApiController]
public class EventUserController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<bool>> CreateAsync(
    [FromServices] ICreateEventUserCommand command,
    [FromBody] CreateEventUserRequest request)
  {
    return await command.ExecuteAsync(request);
  }
}
