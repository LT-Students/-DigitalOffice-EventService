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

  public Task<int> CountCategoriesAsync(Guid eventId)
  {
    return _provider.EventsCategories.AsNoTracking().CountAsync(ec => ec.EventId == eventId);
  }

  public bool DoesExistAsync(Guid eventId, List<Guid> categoriesIds)
  {
    return categoriesIds.All(categoryId =>
      _provider.EventsCategories.AnyAsync(ec => ec.CategoryId == categoryId && ec.EventId == eventId).Result);
  }

  public async Task<bool> RemoveAsync(Guid eventId, List<Guid> categoriesIds)
  {
    if (categoriesIds is null || !categoriesIds.Any())
    {
      return false;
    }

    _provider.EventsCategories.RemoveRange(
      _provider.EventsCategories.Where(ec => categoriesIds.Contains(ec.CategoryId) && ec.EventId == eventId));
    await _provider.SaveAsync();

    return true;
  }
}
