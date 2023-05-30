using System;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Models.Dto.Responses.Event;
using LT.DigitalOffice.Kernel.Responses;
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

  [HttpGet("find")]
  public async Task<FindResultResponse<EventInfo>> FindAsync(
    [FromServices] IFindEventsCommand command,
    [FromQuery] FindEventsFilter filter,
    CancellationToken ct)
  {
    return await command.ExecuteAsync(filter: filter, ct: ct);
  }

  [HttpGet("get")]
  public async Task<OperationResultResponse<EventResponse>> GetAsync(
    [FromServices] IGetEventCommand command,
    [FromQuery] Guid eventId,
    CancellationToken ct)
  {
    return await command.ExecuteAsync(eventId: eventId, ct: ct);
  }
}
