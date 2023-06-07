using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbEventMapper : IDbEventMapper
{
  private readonly IDbImageMapper _imageMapper;

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

  public DbEventMapper(
    IDbImageMapper imageMapper)
  {
    _imageMapper = imageMapper;
  }

  public DbEvent Map(
    CreateEventRequest request,
    Guid senderId,
    List<Guid> imagesIds)
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
        EndDate = request.EndDate,
        Format = request.Format,
        Access = request.Access,
        IsActive = true,
        CreatedBy = senderId,
        CreatedAtUtc = DateTime.UtcNow,
        Users = MapEventUsers(request, senderId, eventId),
        EventsCategories = MapEventCategories(request, senderId, eventId),
        Images = imagesIds?
          .ConvertAll(imageId => _imageMapper.Map(imageId, eventId))
      };
  }
}
