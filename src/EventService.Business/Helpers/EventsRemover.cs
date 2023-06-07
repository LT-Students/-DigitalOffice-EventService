using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Broker.Publishes.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LT.DigitalOffice.EventService.Business.Helpers;

public class EventsRemover
{
  private readonly IServiceScopeFactory _scopeFactory;
  private readonly IPublish _publish;

  private async Task ExecuteAsync()
  {
    using var scope = _scopeFactory.CreateScope();
    using var dbContext = scope.ServiceProvider.GetRequiredService<EventServiceDbContext>();

    DateTime date = DateTime.Now;
    date = date.AddYears(-1);

    List<DbEvent> events = await dbContext.Events
      .Where(e => e.IsActive && ((e.Date <= date && e.EndDate == null) || e.EndDate <= date))
      .Include(e => e.Users)
      .Include(e => e.Files)
      .Include(e => e.Images)
      .Include(e => e.Comments)
        .ThenInclude(c => c.Images)
      .Include(e => e.Comments)
        .ThenInclude(c => c.Files)
      .ToListAsync();

    if (!events.Any())
    {
      return;
    }

    List<Guid> filesIds = new();
    List<Guid> imagesIds = new();

    foreach (DbEvent dbEvent in events)
    {
      dbEvent.IsActive = false;

      filesIds.AddRange(dbEvent.Files.Select(file => file.FileId));
      imagesIds.AddRange(dbEvent.Images.Select(image => image.ImageId));

      IEnumerable<DbEventComment> comments = dbEvent.Comments.Where(x => x.Content != null);

      foreach (DbEventComment comment in comments)
      {
        filesIds.AddRange(comment.Files.Select(f => f.FileId));
        imagesIds.AddRange(comment.Images.Select(f => f.ImageId));

        dbContext.Images.RemoveRange(comment.Images);
        dbContext.Files.RemoveRange(comment.Files);
      }

      dbContext.EventsUsers.RemoveRange(dbEvent.Users);

      dbContext.Images.RemoveRange(dbEvent.Images);

      dbContext.Files.RemoveRange(dbEvent.Files);
    }

    await dbContext.SaveChangesAsync();

    await _publish.RemoveFilesAsync(filesIds);
    await _publish.RemoveImagesAsync(imagesIds);
  }

  public EventsRemover(
    IServiceScopeFactory scopeFactory,
    IPublish publish)
  {
    _scopeFactory = scopeFactory;
    _publish = publish;
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
