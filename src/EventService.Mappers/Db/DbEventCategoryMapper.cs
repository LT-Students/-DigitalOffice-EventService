using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventCategory;
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

  public List<DbEventCategory> Map(CreateEventCategoryRequest request)
  {
    return request is null
      ? null
      : request.CategoryIds.Select(categoryId => new DbEventCategory
      {
        Id = Guid.NewGuid(),
        EventId = request.EventId,
        CategoryId = categoryId,
        CreatedBy = _contextAccessor.HttpContext.GetUserId(),
        CreatedAtUtc = DateTime.UtcNow
      }).ToList();
  }

  public List<DbEventCategory> Map(
    CreateEventRequest request, 
    Guid senderId,
    Guid eventId)
  {
    return request is null
      ? null
      : request.CategoryIds.Select(categoryId => new DbEventCategory
      {
        Id = Guid.NewGuid(),
        EventId = eventId,
        CategoryId = categoryId,
        CreatedBy = senderId,
        CreatedAtUtc = DateTime.UtcNow
      }).ToList();
  }
}
