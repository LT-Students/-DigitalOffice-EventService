using System;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Dto.Responses.Event;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.Event;

public class GetEventCommand : IGetEventCommand
{
  private readonly IEventRepository _repository;
  private readonly IEventInfoMapper _mapper;

  public GetEventCommand(
    IEventRepository repository,
    IEventInfoMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public Task<OperationResultResponse<EventResponse>> ExecuteAsync(Guid eventId, CancellationToken ct)
  {
    throw new NotImplementedException();
  }
}
