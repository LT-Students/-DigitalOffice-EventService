using System;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Mappers.Db
{
  public class DbEventUserMapper : IDbEventUserMapper
  {
    private readonly IHttpContextAccessor _contextAccessor;

    public DbEventUserMapper(IHttpContextAccessor accessor)
    {
      _contextAccessor = accessor;
    }

    public DbEventUser Map(CreateEventUserRequest request)
    {
      return request is null
        ? null
        : new DbEventUser
        {
          Id = Guid.NewGuid(),
          EventId = request.EventId,
          UserId = request.UserId,
          Status = request.UserStatus,
          NotifyAtUtc = request.NotifyAtUtc,
          CreatedBy = _contextAccessor.HttpContext.GetUserId(),
          CreatedAtUtc = DateTime.UtcNow
        };
    }
  }
}
