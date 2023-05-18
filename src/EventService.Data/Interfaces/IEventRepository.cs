using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IEventRepository
{
  Task<Guid?> CreateAsync(DbEvent dbEvent);
  Task<bool> DoesExistAsync(Guid eventId);
  Task<List<Guid>> DoExistAsync(List<Guid> eventsIds);
  Task<DbEvent> GetAsync(Guid eventId);
}
