using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.EventCategory.Interfaces;

[AutoInject]
public interface ICreateEventCategoryCommand
{
  public Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateEventCategoryRequest request);
}
