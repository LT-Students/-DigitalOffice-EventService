using FluentValidation;
using LT.DigitalOffice.Kernel.Validators.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.EventService.Validation.Image.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.Image;

public class ImageValidator : AbstractValidator<ImageContent>, IImageValidator
{
  public ImageValidator(
    IImageContentValidator contentValidator,
    IImageExtensionValidator extensionValidator)
  {
    RuleFor(i => i.Content)
      .SetValidator(contentValidator);

    RuleFor(i => i.Extension)
      .SetValidator(extensionValidator);
  }
}
