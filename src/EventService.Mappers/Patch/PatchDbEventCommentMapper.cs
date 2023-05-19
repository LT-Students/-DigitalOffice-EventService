using LT.DigitalOffice.EventService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.EventService.Mappers.Patch;

public class PatchDbEventCommentMapper : IPatchDbEventCommentMapper
{
  public JsonPatchDocument<DbEventComment> Map(JsonPatchDocument<EditEventCommentRequest> request)
  {
    if (request is null)
    {
      return null;
    }

    JsonPatchDocument<DbEventComment> dbEventCommentPatch = new();

    foreach (Operation<EditEventCommentRequest> item in request.Operations)
    {
      dbEventCommentPatch.Operations.Add(new Operation<DbEventComment>(
        item.op,
        item.path,
        item.from,
        string.IsNullOrEmpty(item.value?.ToString().Trim())
          ? null
          : item.value.ToString().Trim()));
    }

    return dbEventCommentPatch;
  }
}
