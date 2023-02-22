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

  {
    if (dbEventCategories is null)
    {
      return false;
    }

    _provider.EventsCategories.AddRange(dbEventCategories);
    _provider.EventsCategories.RemoveRange(
      _provider.EventsCategories.Where(ec => categoryIds.Contains(ec.CategoryId) && ec.EventId == eventId));
    await _provider.SaveAsync();

    return true;
  }
}
