using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data;

public class EventCategoryRepository : IEventCategoryRepository
{
  private readonly IDataProvider _provider;

  public EventCategoryRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<bool> CreateAsync(List<DbEventCategory> dbEventCategories)
  {
    if (dbEventCategories is null)
    {
      return false;
    }

    _provider.EventsCategories.AddRange(dbEventCategories);
    await _provider.SaveAsync();

    return true;
  }

  public Task<bool> DoesExistAsync(Guid eventId, List<Guid> categoryId)
  {
    return _provider.EventsCategories.AsNoTracking().AnyAsync(ec => categoryId.Contains(ec.CategoryId) && ec.EventId == eventId);
  }

  public Task<int> CountCategoriesAsync(Guid eventId)
  {
    return _provider.EventsCategories.AsNoTracking().CountAsync(ec => ec.EventId == eventId);
  }
}
