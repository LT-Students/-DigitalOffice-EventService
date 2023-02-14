using System;
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

  public async Task<Guid?> CreateAsync(DbEventCategory dbEventCategory)
  {
    if (dbEventCategory is null)
    {
      return null;
    }

    _provider.EventsCategories.Add(dbEventCategory);
    await _provider.SaveAsync();
    return dbEventCategory.Id;
  }

  public Task<bool> DoesExistAsync(Guid eventId, Guid categoryId)
  {
    return _provider.EventsCategories.AsNoTracking().AnyAsync(ec => ec.EventId == eventId && ec.CategoryId == categoryId);
  }

  public async Task<int> CountAsync(Guid eventId)
  {
    return await _provider.EventsCategories.CountAsync(ec => ec.EventId == eventId);
  }
}
