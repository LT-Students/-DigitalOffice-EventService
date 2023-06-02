using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http;
using LT.DigitalOffice.Kernel.Extensions;

namespace LT.DigitalOffice.EventService.Data;

public class EventRepository : IEventRepository
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public EventRepository(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<Guid?> CreateAsync(DbEvent dbEvent)
  {
    if (dbEvent is null)
    {
      return null;
    }

    _provider.Events.Add(dbEvent);
    _provider.EventsUsers.AddRange(dbEvent.Users);

    if (!dbEvent.EventsCategories.IsNullOrEmpty())
    {
      _provider.EventsCategories.AddRange(dbEvent.EventsCategories);
    }

    await _provider.SaveAsync();

    return dbEvent.Id;
  }

  public async Task<bool> EditAsync(Guid eventId, JsonPatchDocument<DbEvent> request)
  {
    DbEvent dbEvent = await _provider.Events.FirstOrDefaultAsync(x => x.Id == eventId);

    if (dbEvent is null || request is null)
    {
      return false;
    }

    bool oldIsActive = dbEvent.IsActive;
    Guid senderId = _httpContextAccessor.HttpContext.GetUserId();

    request.ApplyTo(dbEvent);
    dbEvent.ModifiedBy = senderId;
    dbEvent.ModifiedAtUtc = DateTime.UtcNow;

    bool newIsActive = dbEvent.IsActive;

    if (oldIsActive != newIsActive)
    {
      _provider.EventsUsers.RemoveRange(_provider.EventsUsers.Where(x => x.EventId == eventId));

      List<DbEventComment> comments = _provider.EventComments.Where(x => x.EventId == eventId && (x.Content != null)).ToList();
      foreach (DbEventComment comment in comments)
      {
        comment.IsActive = newIsActive;
        comment.ModifiedBy = senderId;
        comment.ModifiedAtUtc = DateTime.UtcNow;
      }
    }

    await _provider.SaveAsync();

    return true;
  }

  public Task<bool> DoesExistAsync(Guid eventId, bool? isActive)
  {
    if (isActive is not null)
    {
      return _provider.Events.AnyAsync(e => e.Id == eventId && e.IsActive);
    }
    else
    {
      return _provider.Events.AnyAsync(e => e.Id == eventId);
    }
  }

  public Task<bool> IsEventCompletedAsync(Guid eventId)
  {
    return _provider.Events.AnyAsync(
      e => e.Id == eventId &&
      (e.Date > DateTime.UtcNow && e.EndDate == null) ||
      (e.Date > DateTime.UtcNow && e.EndDate > DateTime.UtcNow));
  }

  public Task<List<Guid>> GetExisting(List<Guid> eventsIds)
  {
    return _provider.Events.AsNoTracking().Where(p => eventsIds.Contains(p.Id)).Select(p => p.Id).ToListAsync();
  }

  public Task<DbEvent> GetAsync(Guid eventId)
  {
    return _provider.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == eventId);
  }

  public async Task<(List<Guid> filesIds, List<Guid> imagesIds)> RemoveImagesFilesAsync(Guid eventId)
  {
    DbEvent dbEvent = await _provider.Events
      .Include(x => x.Files)
      .Include(x => x.Images)
      .FirstOrDefaultAsync(p => p.Id == eventId);

    _provider.EventImages.RemoveRange(_provider.EventImages.Where(x => x.EventId == eventId));
    _provider.EventFiles.RemoveRange(_provider.EventFiles.Where(x => x.EventId == eventId));

    List<Guid> filesIds = dbEvent.Files.Select(file => file.FileId).ToList();
    List<Guid> imagesIds = dbEvent.Images.Select(image => image.ImageId).ToList();

    await _provider.SaveAsync();

    return (filesIds, imagesIds);
  }
}
