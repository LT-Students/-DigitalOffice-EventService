using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Mappers.Patch.Interfaces;

[AutoInject]
public interface IPatchDbCategoryMapper
{
  JsonPatchDocument<DbCategory> Map(JsonPatchDocument<EditCategoryRequest> request);
}
