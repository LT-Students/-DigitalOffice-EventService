using System;
using DigitalOffice.Models.Broker.Publishing.Subscriber.User;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;

namespace LT.DigitalOffice.EventService.Mappers.Db
{
  public class DbUserBirthdayMapper : IDbUserBirthdayMapper
  {
    public DbUserBirthday Map(IUpdateUserBirthdayPublish publish)
    {
      return publish is null
        ? null
        : new DbUserBirthday
        {
          UserId = publish.UserId,
          DateOfBirthday = publish.DateOfBirth ?? DateTime.UtcNow,
          IsActive = publish.DateOfBirth is null ? false : true,
          CreatedAtUtc = DateTime.UtcNow
        };
    }
  }
}
