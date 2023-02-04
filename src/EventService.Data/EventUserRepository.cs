using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace LT.DigitalOffice.EventService.Data;

  public class EventUserRepository : IEventUserRepository
  {
    private readonly IDataProvider _provider;

    public EventUserRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> DoesExistAsync(Guid userId, Guid eventId)
    {
      return await _provider.EventsUsers.AnyAsync(eu => eu.EventId == eventId && eu.UserId == userId);
    }

    public async Task<List<Guid>> CreateAsync(List<DbEventUser> dbEventUsers)
    {
      if (dbEventUsers is null)
      {
        return null;
      }

      _provider.EventsUsers.AddRange(dbEventUsers);
      await _provider.SaveAsync();

      return dbEventUsers.Select(user => user.Id).ToList();
    }
  }

