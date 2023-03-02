using System;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbEventMapper : IDbEventMapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public DbEventMapper(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public DbEvent Map(
    CreateEventRequest request,
    Guid senderId)
  {
    return request is null
      ? null
      : new DbEvent
      {
        Id = Guid.NewGuid(),
        Name = request.Name,
        Address = request.Address,
        Description = request.Description,
        Date = request.Date,
        Format = request.Format,
        Access = request.Access,
        IsActive = true,
        CreatedBy = senderId,
        CreatedAtUtc = DateTime.UtcNow
      };
  }
}
