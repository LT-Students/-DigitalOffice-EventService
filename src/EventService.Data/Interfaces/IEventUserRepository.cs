using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface IEventUserRepository
{
  Task<bool> DoesExistAsync(List<Guid> userId, Guid eventId);
  Task<bool> CreateAsync(List<DbEventUser> dbEventUsers);
}
