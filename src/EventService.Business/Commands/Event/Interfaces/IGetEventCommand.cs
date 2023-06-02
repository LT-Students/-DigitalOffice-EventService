using System;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Models.Dto.Responses.Event;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;

[AutoInject]
public interface IGetEventCommand
{
  Task<OperationResultResponse<EventResponse>> ExecuteAsync(GetEventFilter filter, CancellationToken ct);
}
