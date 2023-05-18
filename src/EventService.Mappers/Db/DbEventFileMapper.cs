using System;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbEventFileMapper : IDbEventFileMapper
{
  public DbEventFile Map(Guid fileId, Guid eventId)
  {
    return new DbEventFile
    {
      Id = Guid.NewGuid(),
      FileId = fileId,
      EventId = eventId
    };
  }
}
