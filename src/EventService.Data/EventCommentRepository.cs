using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data;

public class EventCommentRepository : IEventCommentRepository
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public EventCommentRepository(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<Guid?> CreateAsync(DbEventComment dbEventComment)
  {
    if (dbEventComment is null)
    {
      return null;
    }

    _provider.EventComments.Add(dbEventComment);
    await _provider.SaveAsync();

    return dbEventComment.Id;
  }

  public async Task<bool> EditContentAsync(Guid commentId, JsonPatchDocument<DbEventComment> request)
  {
    DbEventComment dbEventComment = await _provider.EventComments.FirstOrDefaultAsync(x => x.Id == commentId);

    if (dbEventComment is null || request is null)
    {
      return false;
    }

    request.ApplyTo(dbEventComment);
    dbEventComment.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
    dbEventComment.ModifiedAtUtc = DateTime.UtcNow;

    await _provider.SaveAsync();

    return true;
  }

  public async Task<(bool, List<Guid> filesIds, List<Guid> imagesIds)> EditIsActiveAsync(Guid commentId, JsonPatchDocument<DbEventComment> request)
  {
    DbEventComment dbEventComment = await _provider.EventComments
      .Include(x => x.Images)
      .Include(x => x.Files)
      .FirstOrDefaultAsync(x => x.Id == commentId);

    if (dbEventComment is null || request is null)
    {
      return default;
    }

    List<Guid> filesIds = dbEventComment.Files.Select(file => file.FileId).ToList();
    List<Guid> imagesIds = dbEventComment.Images.Select(image => image.ImageId).ToList();

    if (!await HasChildCommentsAsync(commentId))
    {
      _provider.EventComments.RemoveRange(_provider.EventComments.Where(ec => ec.Id == commentId).FirstOrDefault());
      _provider.Images.RemoveRange(dbEventComment.Images);
      _provider.Files.RemoveRange(dbEventComment.Files);
    }
    else 
    {
      _provider.Images.RemoveRange(dbEventComment.Images);
      _provider.Files.RemoveRange(dbEventComment.Files);

      dbEventComment.IsActive = false;
      dbEventComment.Content = null;
      dbEventComment.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
      dbEventComment.ModifiedAtUtc = DateTime.UtcNow;
    }

    await _provider.SaveAsync();

    return (true, filesIds, imagesIds);
  }

  public Task<DbEventComment> GetAsync(Guid commentId)
  {
    return _provider.EventComments.AsNoTracking().FirstOrDefaultAsync(ec => ec.Id == commentId);
  }

  public Task<bool> DoesExistAsync(Guid commentId)
  {
    return _provider.EventComments.AnyAsync(ec => ec.Id == commentId && ec.IsActive);
  }

  public Task<List<Guid>> GetExisting(List<Guid> commentsIds)
  {
    return _provider.EventComments.AsNoTracking().Where(p => commentsIds.Contains(p.Id)).Select(p => p.Id).ToListAsync();
  }

  public Task<bool> HasChildCommentsAsync(Guid commentId)
  {
    return _provider.EventComments.AnyAsync(ec => ec.ParentId == commentId);
  }
}
