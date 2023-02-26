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
  bool DoesExistAsync(Guid eventId, List<Guid> categoryIds);
  Task<int> CountCategoriesAsync(Guid eventId);
}
