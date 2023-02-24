using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface ICategoryRepository
{
  bool DoesExistAllAsync(List<Guid> categoryIds);
  Task<bool> IsCategoryExist(Guid eventId);
  Task<Guid?> CreateAsync(DbCategory dbCategory);
}
