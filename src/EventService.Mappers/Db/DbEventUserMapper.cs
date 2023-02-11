using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Mappers.Db;

  public class DbEventUserMapper : IDbEventUserMapper
  {
    private readonly IHttpContextAccessor _contextAccessor;

    public DbEventUserMapper(IHttpContextAccessor accessor)
    {
      _contextAccessor = accessor;
    }

  public List<DbEventUser> Map(
    CreateEventUserRequest request, AccessType access, Guid senderId)
  {
    return request is null
      ? null
      : request.Users.Select(u => new DbEventUser
      {
        Id = Guid.NewGuid(),
        EventId = request.EventId,
        UserId = u.UserId,
        Status = (access == AccessType.Opened && u.UserId == senderId)
          ? EventUserStatus.Participant
          : EventUserStatus.Invited,
        NotifyAtUtc = u.NotifyAtUtc,
        CreatedBy = senderId,
        CreatedAtUtc = DateTime.UtcNow
      }).ToList();
  }
}

