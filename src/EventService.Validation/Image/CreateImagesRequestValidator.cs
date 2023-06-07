using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Image;
using LT.DigitalOffice.EventService.Validation.Image.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.Image;

public class CreateImagesRequestValidator : AbstractValidator<CreateImagesRequest>, ICreateImagesRequestValidator
{
  public CreateImagesRequestValidator(
    IImageValidator imageValidator,
    IEventRepository eventRepository,
    IEventCommentRepository commentRepository)
  {
    RuleLevelCascadeMode = CascadeMode.Stop;

    RuleFor(request => request.Images)
      .NotEmpty()
      .WithMessage("List of images must not be null or empty.")
      .ForEach(image =>
      {
        image
          .NotNull()
          .WithMessage("Image must not be null.")
          .SetValidator(imageValidator);
      });

    RuleFor(request => request.EntityId)
      .NotEmpty()
      .WithMessage("Entity id must not be empty.")
      .MustAsync(async (entityId, _) => await eventRepository.DoesExistAsync(entityId, true) || await commentRepository.DoesExistAsync(entityId))
      .WithMessage("Invalid entity id.");
  }
}
