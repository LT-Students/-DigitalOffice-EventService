using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IImageRepository
{
  Task<List<Guid>> CreateAsync(List<DbImage> images);

  Task<bool> RemoveAsync(List<Guid> imagesIds);
}
