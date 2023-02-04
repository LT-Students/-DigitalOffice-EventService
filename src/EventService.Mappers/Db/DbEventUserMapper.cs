using System;
using System.Collections.Generic;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
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

    public List<DbEventUser> Map(CreateEventUserRequest request, EventUserStatus status = EventUserStatus.Invited)
    {
      List<DbEventUser> result = new List<DbEventUser>();
      foreach (var user in request.Users)
      {
        result.Add(new DbEventUser
        {
          Id = Guid.NewGuid(),
          EventId = request.EventId,
          UserId = user.UserId,
          Status = status,
          NotifyAtUtc = user.NotifyAtUtc,
          CreatedBy = _contextAccessor.HttpContext.GetUserId(),
          CreatedAtUtc = DateTime.UtcNow
        });
      }

      return result;
    }
  }

