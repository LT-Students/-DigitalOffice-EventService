using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Requests.EventComment;
using LT.DigitalOffice.EventService.Validation.EventComment.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.EventService.Validation.EventComment;

public class EditEventCommentRequestValidator : ExtendedEditRequestValidator<Guid, EditEventCommentRequest>, IEditEventCommentRequestValidator
{
  private readonly IEventCommentRepository _repository;

  private void HandleInternalPropertyValidation(
    Operation<EditEventCommentRequest> requestedOperation,
    ValidationContext<(Guid, JsonPatchDocument<EditEventCommentRequest>)> context)
  {
    Context = context;
    RequestedOperation = requestedOperation;

    #region paths

    AddСorrectPaths(
      new List<string>
      {
        nameof(EditEventCommentRequest.Content),
        nameof(EditEventCommentRequest.IsActive),
      });

    AddСorrectOperations(nameof(EditEventCommentRequest.Content), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditEventCommentRequest.IsActive), new List<OperationType> { OperationType.Replace });

    #endregion

    #region Content

    AddFailureForPropertyIf(
      nameof(EditEventCommentRequest.Content),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), "Content must not be empty." },
        { x => x.value?.ToString().Length < 301, "Content is too long." }
      }, CascadeMode.Stop);

    #endregion

    #region IsActive

    AddFailureForPropertyIf(
      nameof(EditEventCommentRequest.IsActive),
      x => x == OperationType.Replace,
      new()
      {
        { x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect IsActive value." },
      });

    #endregion
  }

  public EditEventCommentRequestValidator(IEventCommentRepository repository)
  {
    _repository = repository;

    RuleForEach(x => x.Item2.Operations)
      .Custom(HandleInternalPropertyValidation);

    RuleFor(request => request.Item1)
      .MustAsync(async (commentId, _) => await _repository.DoesExistAsync(commentId))
      .WithMessage("This comment doesn't exist.");
  }
}
