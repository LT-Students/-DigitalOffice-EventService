using System;
using System.Collections.Generic;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
[AutoInject]
public interface IDbEventUserMapper
{
  public List<DbEventUser> Map(CreateEventUserRequest request, AccessType access, Guid senderId);
}
