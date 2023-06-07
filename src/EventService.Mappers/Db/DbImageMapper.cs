using System;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbImageMapper : IDbImageMapper
{
  public DbImage Map(Guid imageId, Guid entityId)
  {
    return new DbImage
    {
      Id = Guid.NewGuid(),
      ImageId = imageId,
      EntityId = entityId
    };
  }
}
