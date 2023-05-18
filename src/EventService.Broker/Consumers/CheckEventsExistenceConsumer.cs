using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.Kernel.BrokerSupport.Broker;
using MassTransit;

namespace LT.DigitalOffice.EventService.Broker;

public class CheckEventsExistenceConsumer : IConsumer<ICheckEventsExistence>
{
  private readonly IEventRepository _eventRepository;

  public CheckEventsExistenceConsumer(
    IEventRepository eventRepository)
  {
    _eventRepository = eventRepository;
  }

  public async Task Consume(ConsumeContext<ICheckEventsExistence> context)
  {
    List<Guid> existEvents = await _eventRepository.GetExisting(context.Message.EventsIds);

    object response = OperationResultWrapper.CreateResponse((_) => ICheckEventsExistence.CreateObj(existEvents), context);

    await context.RespondAsync<IOperationResult<ICheckEventsExistence>>(response);
  }
}
