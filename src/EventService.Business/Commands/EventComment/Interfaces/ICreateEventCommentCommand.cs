using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.EventComment.Interfaces;

[AutoInject]
public interface ICreateEventCommentCommand
{
  Task<OperationResultResponse<Guid?>> ExecuteAsync(CreateEventCommentRequest request);
}
