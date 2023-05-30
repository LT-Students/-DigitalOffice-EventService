using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LT.DigitalOffice.EventService.Controllers;

[Route("[controller]")]
[ApiController]
public class EventController : ControllerBase
{
  [HttpPost("create")]
  public async Task<OperationResultResponse<Guid?>> CreateAsync(
    [FromServices] ICreateEventCommand command,
    [FromBody] CreateEventRequest request)
  {
    return await command.ExecuteAsync(request);
  }

  [HttpPatch("edit")]
  public async Task<OperationResultResponse<bool>> EditAsync(
    [FromServices] IEditEventCommand command,
    [FromQuery] Guid eventId,
    [FromBody] JsonPatchDocument<EditEventRequest> request)
  {
    return await command.ExecuteAsync(eventId, request);
  }
}
