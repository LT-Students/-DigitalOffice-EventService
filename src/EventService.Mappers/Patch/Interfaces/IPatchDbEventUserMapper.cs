using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Mappers.Patch.Interfaces;

[AutoInject]
public interface IPatchDbEventUserMapper
{
  JsonPatchDocument<DbEventUser> Map(JsonPatchDocument<EditEventUserRequest> request);
}
