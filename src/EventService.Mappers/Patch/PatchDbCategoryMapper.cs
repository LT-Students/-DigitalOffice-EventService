using LT.DigitalOffice.EventService.Mappers.Patch.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.EventService.Mappers.Patch;

public class PatchDbCategoryMapper : IPatchDbCategoryMapper
{
  public JsonPatchDocument<DbCategory> Map(JsonPatchDocument<EditCategoryRequest> request)
  {
    if (request is null)
    {
      return null;
    }

    JsonPatchDocument<DbCategory> dbCategoryPatch = new();

    foreach (Operation<EditCategoryRequest> item in request.Operations)
    {
      dbCategoryPatch.Operations.Add(new Operation<DbCategory>(
        item.op,
        item.path,
        item.from,
        string.IsNullOrEmpty(item.value?.ToString().Trim())
          ? null
          : item.value.ToString().Trim()));
    }

    return dbCategoryPatch;
  }
}
