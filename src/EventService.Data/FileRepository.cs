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

public class FileRepository : IFileRepository
{
  private readonly IDataProvider _provider;

  public FileRepository(
    IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<List<Guid>> CreateAsync(List<DbFile> files)
  {
    if (files is null || !files.Any())
    {
      return null;
    }

    _provider.Files.AddRange(files);
    await _provider.SaveAsync();

    return files.ConvertAll(x => x.FileId);
  }

  public async Task<bool> RemoveAsync(List<Guid> filesIds)
  {
    if (filesIds is null || !filesIds.Any())
    {
      return false;
    }

    _provider.Files.RemoveRange(
      await _provider.Files
        .Where(x => filesIds.Contains(x.FileId)).ToListAsync());

    await _provider.SaveAsync();

    return true;
  }

  public async Task<(List<DbFile>, int filesCount)> FindAsync(FindFilesFilter filter)
  {
    if (filter is null)
    {
      return default;
    }

    IQueryable<DbFile> dbFilesQuery = _provider.Files.AsNoTracking();

    return (
      await dbFilesQuery.Where(file => file.EntityId == filter.EntityId)
        .Skip(filter.SkipCount).Take(filter.TakeCount).ToListAsync(),
      await dbFilesQuery.CountAsync());
  }

  public Task<List<DbFile>> GetAsync(List<Guid> filesIds)
  {
    return _provider.Files.AsNoTracking().Where(x => filesIds.Contains(x.FileId)).ToListAsync();
  }

  public async Task<bool> CheckFilesAsync(Guid entityId, List<Guid> filesIds)
  {
    if (filesIds is null || !filesIds.Any())
    {
      return false;
    }

    return !await _provider.Files
      .AnyAsync(p => filesIds.Contains(p.FileId) && p.EntityId != entityId);
  }
}
