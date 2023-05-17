using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Mappers.Db.Interfaces;

[AutoInject]
public interface IDbEventCommentMapper
{
  public DbEventComment Map(CreateEventCommentRequest request);
}
