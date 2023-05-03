using System;
using System.Threading.Tasks;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces
{
  [AutoInject]
  public interface IUserBirthdayRepository
  {
    Task UpdateUserBirthdayAsync(Guid userId, DateTime? dateOfBirth);
  }
}
