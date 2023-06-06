using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using LT.DigitalOffice.EventService.Validation.File.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.File;

public class RemoveFilesRequestValidator : AbstractValidator<RemoveFilesRequest>, IRemoveFilesRequestValidator
{
  public RemoveFilesRequestValidator(
    IFileRepository fileRepository)
  {
    RuleLevelCascadeMode = CascadeMode.Stop;

    RuleFor(request => request.FilesIds)
      .NotEmpty()
      .WithMessage("List of files ids must not be null or empty.");

    RuleFor(request => request)
      .MustAsync((x, _) => fileRepository.CheckFilesAsync(x.EntityId, x.FilesIds))
      .WithMessage("All file ids must belong to the same event.");
  }
}
