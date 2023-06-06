using System;
using System.Collections.Generic;
using LT.DigitalOffice.EventService.Mappers.Db.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;

namespace LT.DigitalOffice.EventService.Mappers.Db;

public class DbEventCommentMapper : IDbEventCommentMapper
{
  private readonly IDbImageMapper _imageMapper;
  public DbEventCommentMapper(
    IDbImageMapper imageMapper)
  {
    _imageMapper = imageMapper;
  }

  public DbEventComment Map(CreateEventCommentRequest request, List<Guid> imagesIds)
  {
    Guid commentId = Guid.NewGuid();

    return request is null
      ? null
      : new DbEventComment
      {
        Id = commentId,
        Content = request.Content,
        UserId = request.UserId,
        EventId = request.EventId,
        ParentId = request.ParentId,
        IsActive = true,
        CreatedAtUtc = DateTime.UtcNow,
        Images = imagesIds?
          .ConvertAll(imageId => _imageMapper.Map(imageId, commentId))
      };
  }
}
