using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventCategory;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Validation.EventCategory.Interfaces;

[AutoInject]
public interface IRemoveEventCategoryRequestValidator : IValidator<RemoveEventCategoryRequest>
{
}
