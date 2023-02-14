using System;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbEventCategoryMapper : IDbEventCategoryMapper
{
  private readonly IHttpContextAccessor _contextAccessor;

  public DbEventCategoryMapper(IHttpContextAccessor accessor)
  {
    _contextAccessor = accessor;
  }

  public DbEventCategory Map(CreateEventCategoryRequest request)
  {
    return request is null
      ? null
      : new DbEventCategory
      {
        Id = Guid.NewGuid(),
        EventId = request.EventId,
        CategoryId = request.CategoryId,
        CreatedBy = _contextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow
      };
  }
}
