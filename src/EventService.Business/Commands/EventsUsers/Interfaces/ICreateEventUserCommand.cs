using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventsUsers;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.EventsUsers.Interfaces
{
  [AutoInject]
  public interface ICreateEventUserCommand
  {
    public Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateEventUserRequest request);
  }
}
