using DigitalOffice.Models.Broker.Publishing.Subscriber.User;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbUserBirthdayMapper
  {
    DbUserBirthday Map(IUpdateUserBirthdayPublish publish);
  }
}
