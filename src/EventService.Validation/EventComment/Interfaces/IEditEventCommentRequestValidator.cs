using System;
using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Validation.EventComment.Interfaces;

[AutoInject]
public interface IEditEventCommentRequestValidator : IValidator<(Guid, JsonPatchDocument<EditEventCommentRequest>)>
{
}
