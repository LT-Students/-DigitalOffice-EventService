using System;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbEventMapper : IDbEventMapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public DbEventMapper(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public DbEvent Map(CreateEventRequest request)
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
        CreatedBy = _httpContextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow
      };
  }
}
