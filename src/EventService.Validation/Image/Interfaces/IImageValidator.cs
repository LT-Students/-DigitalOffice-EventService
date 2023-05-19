using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Validation.Image.Interfaces;

[AutoInject]
public interface IImageValidator : IValidator<ImageContent>
{
}
