using LT.DigitalOffice.EventService.Mappers.Models.Interface;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Models;

namespace LT.DigitalOffice.EventService.Mappers.Models;

public class CommentInfoMapper : ICommentInfoMapper
{
  public CommentInfo Map(DbEventComment dbComment)
  {
    return dbComment is null
    ? null
    : new CommentInfo
    {
      Id = dbComment.Id,
      Content = dbComment.Content,
      UserId = dbComment.UserId,
      ParentId = dbComment.ParentId,
      Comment = new()
    };
  }
}
