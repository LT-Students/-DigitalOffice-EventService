using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Validation.Category.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.Category;

public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>, ICreateCategoryValidator
{
  public CreateCategoryValidator()
  {
    RuleFor(request => request.Name)
      .Cascade(CascadeMode.Stop)
      .NotEmpty()
      .WithMessage("Name is empty")
      .MinimumLength(1)
      .WithMessage("Name is too short")
      .MaximumLength(20)
      .WithMessage("Name is too long");
    RuleFor(request => request.Color)
      .IsInEnum();
  }
}

