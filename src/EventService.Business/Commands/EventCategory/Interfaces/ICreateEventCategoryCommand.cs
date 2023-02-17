using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventCategory;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.EventCategory.Interfaces;

[AutoInject]
public interface ICreateEventCategoryCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(CreateEventCategoryRequest request);
}
