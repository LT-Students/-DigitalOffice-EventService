using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Validation.EventComment.Interfaces;

[AutoInject]
public interface ICreateEventCommentRequestValidator : IValidator<CreateEventCommentRequest>
{
}
