using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Mappers.Patch.Interfaces;

[AutoInject]
public interface IPatchDbEventCommentMapper
{
  JsonPatchDocument<DbEventComment> Map(JsonPatchDocument<EditEventCommentRequest> request);
}
