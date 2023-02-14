using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IEventCategoryRepository
{
  public Task<Guid?> CreateAsync(DbEventCategory dbEventCategory);
  public Task<bool> DoesExistAsync(Guid eventId, Guid categoryId);
  public Task<int> CountAsync(Guid eventId);
}
