using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

  [AutoInject]
  public interface IEventUserRepository
  {
    public Task<bool> IsUserAddedToEventAsync(Guid userId, Guid eventId);
    public Task<Guid?> CreateAsync(DbEventUser dbEventUser);
  }

