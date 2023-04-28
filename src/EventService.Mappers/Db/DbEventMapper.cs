using System;
using System.Collections.Generic;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbEventMapper : IDbEventMapper
{
  private List<DbEventUser> MapEventUsers(
    CreateEventRequest request,
    Guid senderId,
    Guid eventId)
  {
    return request.Users.ConvertAll(u => new DbEventUser
    {
      Id = Guid.NewGuid(),
      EventId = eventId,
      UserId = u.UserId,
      Status = u.UserId == senderId
        ? EventUserStatus.Participant
        : EventUserStatus.Invited,
      NotifyAtUtc = u.NotifyAtUtc,
      CreatedBy = senderId,
      CreatedAtUtc = DateTime.UtcNow
    });
  }

  private List<DbEventCategory> MapEventCategories(
    CreateEventRequest request,
    Guid senderId,
    Guid eventId)
  {
    return request.CategoriesIds is null
          ? null
          : request.CategoriesIds.ConvertAll(categoryId => new DbEventCategory
          {
            Id = Guid.NewGuid(),
            EventId = eventId,
            CategoryId = categoryId,
            CreatedBy = senderId,
            CreatedAtUtc = DateTime.UtcNow
          });
  }

  public DbEvent Map(
    CreateEventRequest request,
    Guid senderId)
  {
    Guid eventId = Guid.NewGuid();

    return request is null
      ? null
      : new DbEvent
      {
        Id = eventId,
        Name = request.Name,
        Address = request.Address,
        Description = request.Description,
        Date = request.Date,
        Format = request.Format,
        Access = request.Access,
        IsActive = true,
        CreatedBy = senderId,
        CreatedAtUtc = DateTime.UtcNow,
        Users = MapEventUsers(request, senderId, eventId),
        EventsCategories = MapEventCategories(request, senderId, eventId),
      };
  }
}
