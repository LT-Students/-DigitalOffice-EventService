using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.File;
using LT.DigitalOffice.EventService.Validation.File.Interfaces;

namespace LT.DigitalOffice.EventService.Validation.File;

public class RemoveFilesRequestValidator : AbstractValidator<RemoveFilesRequest>, IRemoveFilesRequestValidator
{
  public RemoveFilesRequestValidator(
    IEventFileRepository projectFileRepository)
  {
    CascadeMode = CascadeMode.Stop;

    RuleFor(request => request.FilesIds)
      .NotNull().WithMessage("List of files ids must not be null.")
      .NotEmpty().WithMessage("List of files ids must not be empty.");

    RuleFor(request => request)
      .MustAsync(async (x, _) => await projectFileRepository.CheckEventFilesAsync(x.EventId, x.FilesIds))
      .WithMessage("All file ids must belong to the same event.");
  }
}
