using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Image;
using LT.DigitalOffice.EventService.Validation.Image.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.Image;

public class CreateImagesRequestValidator : AbstractValidator<CreateImagesRequest>, ICreateImagesRequestValidator
{
  public CreateImagesRequestValidator(
    IImageValidator imageValidator,
    IEventRepository eventRepository)
  {
    CascadeMode = CascadeMode.Stop;

    RuleFor(request => request.Images)
      .NotEmpty().WithMessage("List of images must not be null or empty.")
      .ForEach(image =>
      {
        image
          .NotNull().WithMessage("Image must not be null.")
          .SetValidator(imageValidator);
      });

    RuleFor(request => request.EventId)
      .NotEmpty().WithMessage("Event id must not be empty.")
      .MustAsync(async (e, _) => await eventRepository.DoesExistAsync(e))
      .WithMessage("Invalid event id.");
  }
}
