using System.Collections.Generic;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Db.Interfaces;

  [AutoInject]
	public interface IDbEventUserMapper
  {
    List<DbEventUser> Map(CreateEventUserRequest request, EventUserStatus status = EventUserStatus.Invited);
  }

