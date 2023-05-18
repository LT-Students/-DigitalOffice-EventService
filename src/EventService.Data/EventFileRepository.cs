using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data;

public class EventFileRepository : IEventFileRepository
{
  private readonly IDataProvider _provider;

  public EventFileRepository(
    IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<List<Guid>> CreateAsync(List<DbEventFile> files)
  {
    if (files is null || !files.Any())
    {
      return null;
    }

    _provider.EventFiles.AddRange(files);
    await _provider.SaveAsync();

    return files.Select(x => x.FileId).ToList();
  }

  public async Task<bool> RemoveAsync(List<Guid> filesIds)
  {
    if (filesIds is null || !filesIds.Any())
    {
      return false;
    }

    _provider.EventFiles.RemoveRange(
      _provider.EventFiles
        .Where(x => filesIds.Contains(x.FileId)));

    await _provider.SaveAsync();

    return true;
  }

  public async Task<(List<DbEventFile>, int filesCount)> FindAsync(FindEventFilesFilter filter)
  {
    if (filter is null)
    {
      return default;
    }

    IQueryable<DbEventFile> dbFilesQuery = _provider.EventFiles
      .AsNoTracking();

    return (
      await dbFilesQuery.Where(file => file.EventId == filter.EventId)
        .Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync(),
      await dbFilesQuery.CountAsync());
  }

  public Task<List<DbEventFile>> GetAsync(List<Guid> filesIds)
  {
    return _provider.EventFiles.AsNoTracking().Where(x => filesIds.Contains(x.FileId)).ToListAsync();
  }

  public async Task<bool> CheckEventFilesAsync(Guid eventId, List<Guid> filesIds)
  {
    if (filesIds is null || !filesIds.Any())
    {
      return false;
    }

    return !await _provider.EventFiles
      .AnyAsync(p => filesIds.Contains(p.FileId) && p.EventId != eventId);
  }
}
