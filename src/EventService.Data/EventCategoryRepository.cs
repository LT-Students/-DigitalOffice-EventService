using System;
using System.Collections.Generic;
using System.Linq;
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

  public bool DoesExistAsync(Guid eventId, List<Guid> categoryIds)
  {
    return categoryIds.All(categoryId => _provider.EventsCategories.AnyAsync(ec => ec.CategoryId == categoryId && ec.EventId == eventId).Result);
  }

  public Task<int> CountCategoriesAsync(Guid eventId)
  {
    return _provider.EventsCategories.AsNoTracking().CountAsync(ec => ec.EventId == eventId);
  }
}
