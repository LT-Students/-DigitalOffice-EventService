using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;

[AutoInject]
public interface IEditCategoryCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(Guid categoryId, JsonPatchDocument<EditCategoryRequest> patch);
}
