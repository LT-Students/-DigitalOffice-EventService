using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Business.Commands.EventComment.Interfaces;

[AutoInject]
public interface IEditEventCommentCommand
{
  Task<OperationResultResponse<bool>> ExecuteAsync(Guid commentId, JsonPatchDocument<EditEventCommentRequest> patch);
}
