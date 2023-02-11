using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data;

  public class EventUserRepository : IEventUserRepository
  {
    private readonly IDataProvider _provider;

    public EventUserRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> DoesExistAsync(List<Guid> userId, Guid eventId)
    {
      return await _provider.EventsUsers.AnyAsync(eu => userId.Contains(eu.UserId) && eu.EventId == eventId);
    }

    public async Task<bool> CreateAsync(List<DbEventUser> dbEventUsers)
    {
      if (dbEventUsers is null)
      {
        return false;
      }

      _provider.EventsUsers.AddRange(dbEventUsers);
      await _provider.SaveAsync();

      return true;
    }
  }

