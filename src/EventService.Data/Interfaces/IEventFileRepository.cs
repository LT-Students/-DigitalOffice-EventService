using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IEventFileRepository
{
  Task<List<Guid>> CreateAsync(List<DbEventFile> files);

  Task<bool> RemoveAsync(List<Guid> filesIds);

  Task<(List<DbEventFile>, int filesCount)> FindAsync(FindEventFilesFilter filter);

  Task<List<DbEventFile>> GetAsync(List<Guid> filesIds);

  Task<bool> CheckEventFilesAsync(Guid eventId, List<Guid> filesIds);
}
