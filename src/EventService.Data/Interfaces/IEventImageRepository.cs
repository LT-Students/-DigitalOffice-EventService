using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IEventImageRepository
{
  Task<List<Guid>> CreateAsync(List<DbEventImage> images);

  Task<bool> RemoveAsync(List<Guid> imagesIds);
}
