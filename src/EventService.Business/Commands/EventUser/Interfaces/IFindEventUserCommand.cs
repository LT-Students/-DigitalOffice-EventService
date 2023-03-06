using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser.Filter;
using LT.DigitalOffice.Kernel.Responses;
using System.Threading.Tasks;
using System.Threading;
using System;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Business.Commands.EventUser.Interfaces;

[AutoInject]
public interface IFindEventUserCommand
{
  Task<FindResultResponse<EventUserInfo>> ExecuteAsync(
    Guid eventId,
    FindEventUsersFilter filter,
    CancellationToken cancellationToken = default);
}
