using System;
using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;

namespace LT.DigitalOffice.EventService.Mappers.Models;

public class UserBirthdayInfoMapper : IUserBirthdayInfoMapper
{
  public UserBirthdayInfo Map(DbUserBirthday userBirthday, DateTime dateOfBirth)
  {
    return userBirthday is null
      ? null
      : new UserBirthdayInfo
      {
        UserId = userBirthday.UserId,
        DateOfBirth = dateOfBirth,
      };
  }
}
