using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data;

public class EventCategoryRepository : IEventCategoryRepository
{
  private readonly IDataProvider _provider;

  public EventCategoryRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public bool DoesExistAsync(Guid eventId, List<Guid> categoryIds)
  {
    return categoryIds.All(categoryId => _provider.EventsCategories.AnyAsync(ec => ec.CategoryId == categoryId && ec.EventId == eventId).Result);
  }

  public async Task<bool> RemoveAsync(Guid eventId, List<Guid> categoryIds)
  {
    if (categoryIds is null || !categoryIds.Any())
    {
      return false;
    }

    _provider.EventsCategories.RemoveRange(_provider.EventsCategories.Where(ec => categoryIds.Contains(ec.CategoryId) && ec.EventId == eventId));
    await _provider.SaveAsync();

    return true;
  }
}
