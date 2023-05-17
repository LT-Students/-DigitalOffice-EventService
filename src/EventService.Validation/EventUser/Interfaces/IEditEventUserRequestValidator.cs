using System;
using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventUser;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Validation.EventUser.Interfaces;

[AutoInject]
public interface IEditEventUserRequestValidator : IValidator<(Guid, JsonPatchDocument<EditEventUserRequest>)>
{
}
