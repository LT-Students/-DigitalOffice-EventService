using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Mappers.Patch.Interfaces;

[AutoInject]
public interface IPatchDbEventMapper
{
  JsonPatchDocument<DbEvent> Map(JsonPatchDocument<EditEventRequest> request);
}
