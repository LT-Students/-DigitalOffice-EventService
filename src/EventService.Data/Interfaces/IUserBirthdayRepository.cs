using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces
{
  [AutoInject]
  public interface IUserBirthdayRepository
  {
    Task UpdateUserBirthdatAsync(DbUserBirthday usersBirthdays);
  }
}
