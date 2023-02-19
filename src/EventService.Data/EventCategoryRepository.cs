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

  public async Task<bool> DoesExistAsync(Guid eventId, List<Guid> categoryIds)
  {
    return await _provider.EventsCategories.AsNoTracking().AnyAsync(ec => categoryIds.Contains(ec.CategoryId) && ec.EventId == eventId);
  }

  public async Task<bool> RemoveAsync(Guid eventId, List<Guid> categoryIds)
  {
    if (categoryIds is null)
    {
      return false;
    }

    IQueryable<DbEventCategory> eventCategories = _provider.EventsCategories.Where(ec => categoryIds.Contains(ec.CategoryId) && ec.EventId == eventId);
    _provider.EventsCategories.RemoveRange(eventCategories);
    await _provider.SaveAsync();

    return true;
  }
}
