using System;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbEventCommentMapper : IDbEventCommentMapper
{
  public DbEventComment Map(CreateEventCommentRequest request)
  {
    return request is null
      ? null
      : new DbEventComment
      {
        Id = Guid.NewGuid(),
        Content = request.Content,
        UserId= request.UserId,
        EventId= request.EventId,
        ParentId= request.ParentId,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow
      };
  }
}
