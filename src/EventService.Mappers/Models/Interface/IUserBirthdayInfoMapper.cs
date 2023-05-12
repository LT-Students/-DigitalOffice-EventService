﻿using System;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Models.Interface;

[AutoInject]
public interface IUserBirthdayInfoMapper
{
  UserBirthdayInfo Map(DbUserBirthday userBirthday, DateTime dateOfBirth);
}
