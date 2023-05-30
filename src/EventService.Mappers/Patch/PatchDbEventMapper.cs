using LT.DigitalOffice.EventService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.EventService.Mappers.Patch;

public class PatchDbEventMapper : IPatchDbEventMapper
{
  public JsonPatchDocument<DbEvent> Map(JsonPatchDocument<EditEventRequest> request)
  {
    if (request is null)
    {
      return null;
    }

    JsonPatchDocument<DbEvent> dbEventPatch = new();

    foreach (Operation<EditEventRequest> item in request.Operations)
    {
      dbEventPatch.Operations.Add(new Operation<DbEvent>(
        item.op,
        item.path,
        item.from,
        string.IsNullOrEmpty(item.value?.ToString().Trim())
          ? null
          : item.value.ToString().Trim()));
    }

    return dbEventPatch;
  }
}
