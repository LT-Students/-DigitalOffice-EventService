using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Business.Commands.EventUser.Interfaces;

[AutoInject]
public interface IEditEventUserCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(Guid eventUserId, JsonPatchDocument<EditEventUserRequest> patch);
}
