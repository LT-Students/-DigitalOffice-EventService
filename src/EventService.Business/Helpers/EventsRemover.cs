using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LT.DigitalOffice.EventService.Business.Helpers;

public class EventsRemover
{
  private readonly IServiceScopeFactory _scopeFactory;

  private async Task ExecuteAsync()
  {
    using var scope = _scopeFactory.CreateScope();
    using var dbContext = scope.ServiceProvider.GetRequiredService<EventServiceDbContext>();

    DateTime date = DateTime.Now;
    date = date.AddYears(-1);

    List<DbEvent> events = await dbContext.Events
      .Where(e => e.IsActive && e.EndDate <= date)
      .Include(e => e.Files)
      .Include(e => e.Images)
      .ToListAsync();

    if (!events.Any())
    {
      return;
    }

    foreach (DbEvent dbEvent in events)
    {
      dbEvent.IsActive = false;
    }

    await dbContext.SaveChangesAsync();
  }

  public EventsRemover(
    IServiceScopeFactory scopeFactory)
  {
    _scopeFactory = scopeFactory;
  }

  public void Start()
  {
    Task.Run(async () =>
    {
      while (true)
      {
        if (DateTime.UtcNow.Day == DateTime.DaysInMonth(DateTime.UtcNow.Year, month: DateTime.UtcNow.Month))
        {
          await ExecuteAsync();
        }

        await Task.Delay(TimeSpan.FromDays(1));
      }
    });
  }
}
