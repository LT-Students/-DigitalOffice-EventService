using System;
using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Validation.Event.Interfaces;

[AutoInject]
public interface IEditEventRequestValidator : IValidator<(Guid, JsonPatchDocument<EditEventRequest>)>
{
}
