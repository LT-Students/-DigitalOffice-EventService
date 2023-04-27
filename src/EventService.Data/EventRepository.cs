using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LT.DigitalOffice.EventService.Data;

public class EventRepository : IEventRepository
{
  private readonly IDataProvider _provider;

  public EventRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<Guid?> CreateAsync(DbEvent dbEvent)
  {
    if (dbEvent is null)
    {
      return null;
    }

    _provider.Events.Add(dbEvent);
    _provider.EventsUsers.AddRange(dbEvent.Users);

    if (!dbEvent.EventsCategories.IsNullOrEmpty())
    {
      _provider.EventsCategories.AddRange(dbEvent.EventsCategories);
    }

    await _provider.SaveAsync();

    return dbEvent.Id;
  }
  public Task<bool> DoesExistAsync(Guid eventId)
  {
    return _provider.Events.AnyAsync(e => e.Id == eventId && e.IsActive);
  }

  public async Task<DbEvent> GetAsync(Guid eventId)
  {
    return await _provider.Events.FirstOrDefaultAsync(e => e.Id == eventId);
  }
}
