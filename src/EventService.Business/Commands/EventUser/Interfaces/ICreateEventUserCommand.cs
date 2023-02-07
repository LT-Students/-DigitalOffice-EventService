using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.EventUser.Interfaces;

  [AutoInject]
  public interface ICreateEventUserCommand
  {
    public Task<OperationResultResponse<bool>> ExecuteAsync(CreateEventUserRequest request);
  }

