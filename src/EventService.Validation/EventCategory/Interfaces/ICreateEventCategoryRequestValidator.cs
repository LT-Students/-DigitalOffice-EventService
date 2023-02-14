using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Validation.EventCategory.Interfaces;

[AutoInject]
public interface ICreateEventCategoryRequestValidator : IValidator<CreateEventCategoryRequest>
{
}

