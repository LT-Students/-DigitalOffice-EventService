using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data
{
  public class EventUserRepository : IEventUserRepository
  {
    private readonly IDataProvider _provider;

    public EventUserRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> IsUserAddedToEventAsync(Guid userId, Guid eventId)
    {
      return await _provider.EventsUsers.AnyAsync(eu => eu.EventId == eventId && eu.UserId == userId);
    }

    public async Task<Guid?> CreateAsync(DbEventUser dbEventUser)
    {
      if (dbEventUser is null)
      {
        return null;
      }

      _provider.EventsUsers.Add(dbEventUser);
      await _provider.SaveAsync();

      return dbEventUser.Id;
    }
  }
}
