using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.EventService.Validation.EventCategory.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.EventCategory;

public class CreateEventCategoryRequestValidator : AbstractValidator<CreateEventCategoryRequest>, ICreateEventCategoryRequestValidator
{
  public CreateEventCategoryRequestValidator(
    IEventRepository eventRepository,
    ICategoryRepository categoryRepository,
    IEventCategoryRepository eventCategoryRepository)
  {
    RuleFor(x => x.EventId)
      .MustAsync(async (eventId, _) => await eventRepository.DoesExistAsync(eventId))
      .WithMessage("This event doesn't exist")
      .MustAsync(async (eventId, _) => await eventCategoryRepository.CountAsync(eventId) < 5)
      .WithMessage("This event has 5 categories");

    RuleFor(x => x.CategoryId)
        .MustAsync(async (categoryId, _) => await categoryRepository.DoesExistAsync(categoryId))
        .WithMessage("This category doesn't exist");

    RuleFor(x => x)
        .MustAsync(async (x, _) => !await eventCategoryRepository.DoesExistAsync(x.EventId, x.CategoryId))
        .WithMessage("This entry already exists");
  }
}
