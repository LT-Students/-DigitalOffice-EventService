using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;

[AutoInject]
public interface IFindEventsCommand
{
  Task<FindResultResponse<EventInfo>> ExecuteAsync(
    FindEventsFilter filter,
    CancellationToken ct = default);
}
