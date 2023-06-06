using System;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbFileMapper : IDbFileMapper
{
  public DbFile Map(Guid fileId, Guid entityId)
  {
    return new DbFile
    {
      Id = Guid.NewGuid(),
      FileId = fileId,
      EntityId = entityId
    };
  }
}
