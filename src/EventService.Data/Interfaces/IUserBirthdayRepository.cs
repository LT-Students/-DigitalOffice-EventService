using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces
{
  [AutoInject]
  public interface IUserBirthdayRepository
  {
    Task UpdateUserBirthdayAsync(Guid userId, DateTime? dateOfBirth);
    Task<List<DbUserBirthday>> FindAsync(CancellationToken cancellationToken);
  }
}
