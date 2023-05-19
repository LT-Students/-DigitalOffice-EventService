using System;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbEventImageMapper : IDbEventImageMapper
{
  public DbEventImage Map(Guid imageId, Guid eventId)
  {
    return new DbEventImage
    {
      Id = Guid.NewGuid(),
      ImageId = imageId,
      EventId = eventId
    };
  }
}
