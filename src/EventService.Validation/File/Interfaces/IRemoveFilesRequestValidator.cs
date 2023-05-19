using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Validation.File.Interfaces;

[AutoInject]
public interface IRemoveFilesRequestValidator : IValidator<RemoveFilesRequest>
{
}
