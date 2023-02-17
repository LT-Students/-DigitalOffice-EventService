using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IEventCategoryRepository
{
  Task<bool> CreateAsync(List<DbEventCategory> dbEventCategory);
  Task<bool> DoesExistAsync(Guid eventId, List<Guid> categoryId);
  Task<int> CountAsync(Guid eventId);
}
