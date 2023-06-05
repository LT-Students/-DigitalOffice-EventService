using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Business.Commands.Event.Interfaces;

[AutoInject]
public interface IEditEventCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(Guid eventId, JsonPatchDocument<EditEventRequest> patch);
}
