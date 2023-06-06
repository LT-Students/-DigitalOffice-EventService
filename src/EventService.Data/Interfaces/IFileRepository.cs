using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IFileRepository
{
  Task<List<Guid>> CreateAsync(List<DbFile> files);

  Task<bool> RemoveAsync(List<Guid> filesIds);

  Task<(List<DbFile>, int filesCount)> FindAsync(FindFilesFilter filter);

  Task<List<DbFile>> GetAsync(List<Guid> filesIds);

  Task<bool> CheckFilesAsync(Guid eventId, List<Guid> filesIds);
}
