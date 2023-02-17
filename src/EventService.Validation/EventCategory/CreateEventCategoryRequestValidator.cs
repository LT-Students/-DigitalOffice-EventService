using System.Linq;
using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventCategory;
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
      .WithMessage("This event doesn't exist.");

    RuleFor(x => x.CategoryIds)
      .MustAsync(async (categories, _) => 
      (await categoryRepository.DoesExistAsync(categories.ToList())))
      .WithMessage("This category doesn't exist.");

    RuleFor(x => x)
      .MustAsync(async (x, _) => !await eventCategoryRepository.DoesExistAsync(x.EventId, x.CategoryIds.ToList()))
      .WithMessage("This event already belongs to this category.")
      .MustAsync(async (x, _) => await eventCategoryRepository.CountAsync(x.EventId) + x.CategoryIds.Count <= 5)
      .WithMessage("This event already has 5 categories.");
  }
}
