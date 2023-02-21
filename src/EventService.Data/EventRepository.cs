﻿using System;
using System.Threading.Tasks;
using EventService.Data.Provider;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data;

public class EventRepository : IEventRepository
{
  private readonly IDataProvider _provider;

  public EventRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<bool> CreateAsync(DbEvent dbEvent)
  {
    if (dbEvent == null)
    {
      return false;
    }

    _provider.Events.Add(dbEvent);
    await _provider.SaveAsync();

    return true;
  }
  public Task<bool> DoesExistAsync(Guid eventId)
  {
    return _provider.Events.AsNoTracking().AnyAsync(e => e.Id == eventId);
  }

  public async Task<DbEvent> GetAsync(Guid eventId)
  {
    return await _provider.Events.FirstOrDefaultAsync(e => e.Id == eventId);
  }
}
