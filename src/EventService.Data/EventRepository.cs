using System.Threading.Tasks;
using EventService.Data.Provider;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;

namespace LT.DigitalOffice.EventService.Data;

public class EventRepository : IEventRepository
{
  private readonly IDataProvider _provider;

  public EventRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public Task CreateAsync(DbEvent dbEvent)
  {
    _provider.Events.Add(dbEvent);
    return _provider.SaveAsync();
  }
}
