using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.Event;

public class FindEventsCommand : IFindEventsCommand
{
  private readonly IEventRepository _repository;
  private readonly IEventInfoMapper _mapper;

  public FindEventsCommand(
    IEventRepository repository,
    IEventInfoMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<FindResultResponse<EventInfo>> ExecuteAsync(FindEventsFilter filter, CancellationToken ct = default)
  {
    (List<DbEvent> events, int totalCount) =
      await _repository.FindAsync(filter: filter, ct: ct);

    if (events is null || !events.Any())
    {
      return new();
    }

    return new FindResultResponse<EventInfo>
    {
      Body = events.ConvertAll(_mapper.Map),
      TotalCount = totalCount
    };
  }
}
