﻿using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Image;
using LT.DigitalOffice.EventService.Validation.Image.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.Image;

public class RemoveImagesRequestValidator : AbstractValidator<RemoveImageRequest>, IRemoveImagesRequestValidator
{
  public RemoveImagesRequestValidator()
  {
    RuleFor(request => request.ImagesIds)
      .NotNull().WithMessage("List of images ids must not be null.")
      .NotEmpty().WithMessage("List of images ids must not be empty.")
      .ForEach(x =>
        x.NotEmpty().WithMessage("Image Id must not be empty."));
  }
}
