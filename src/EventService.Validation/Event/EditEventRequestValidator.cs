using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Event;
using LT.DigitalOffice.EventService.Validation.Event.Interfaces;
using LT.DigitalOffice.Kernel.Validators;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.EventService.Validation.Event;

public class EditEventRequestValidator : ExtendedEditRequestValidator<Guid, EditEventRequest>, IEditEventRequestValidator
{
  private void HandleInternalPropertyValidation(
    Operation<EditEventRequest> requestedOperation,
    ValidationContext<(Guid, JsonPatchDocument<EditEventRequest>)> context)
  {
    Context = context;
    RequestedOperation = requestedOperation;

    #region paths

    AddСorrectPaths(
      new List<string>
      {
        nameof(EditEventRequest.Name),
        nameof(EditEventRequest.Address),
        nameof(EditEventRequest.Description),
        nameof(EditEventRequest.Date),
        nameof(EditEventRequest.EndDate),
        nameof(EditEventRequest.Format),
        nameof(EditEventRequest.IsActive),
      });

    AddСorrectOperations(nameof(EditEventRequest.Name), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditEventRequest.Address), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditEventRequest.Description), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditEventRequest.Date), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditEventRequest.EndDate), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditEventRequest.Format), new List<OperationType> { OperationType.Replace });
    AddСorrectOperations(nameof(EditEventRequest.IsActive), new List<OperationType> { OperationType.Replace });

    #endregion

    #region Name

    AddFailureForPropertyIf(
      nameof(EditEventRequest.Name),
      x => x == OperationType.Replace,
      new()
      {
        { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), "Name must not be empty." },
        { x => x.value?.ToString().Length < 151, "Name is too long." }
      }, CascadeMode.Stop);

    #endregion

    #region Address

    AddFailureForPropertyIf(
      nameof(EditEventRequest.Address),
      x => x == OperationType.Replace,
      new()
      {
        { x => x.value?.ToString().Length < 401, "Address is too long." }
      }, CascadeMode.Stop);

    #endregion

    #region Description

    AddFailureForPropertyIf(
      nameof(EditEventRequest.Description),
      x => x == OperationType.Replace,
      new()
      {
        { x => x.value?.ToString().Length < 501, "Description is too long." }
      }, CascadeMode.Stop);

    #endregion

    #region Date

    AddFailureForPropertyIf(
      nameof(EditEventRequest.Date),
      x => x == OperationType.Replace,
      new()
      {
        { x => string.IsNullOrEmpty(x.value?.ToString().Trim()) || DateTime.TryParse(x.value?.ToString().Trim(), out _), "Incorrect date value." },
        { x => (DateTime.TryParse(x.value.ToString().Trim(), out DateTime date) &&
                date > DateTime.UtcNow), "Date must be later than the date the event was created." }
      }, CascadeMode.Stop);

    #endregion

    #region EndDate

    AddFailureForPropertyIf(
      nameof(EditEventRequest.EndDate),
      x => x == OperationType.Replace,
      new()
      {
        { x => x.value is null || (DateTime.TryParse(x.value.ToString().Trim(), out _)), "Incorrect end date value." }
      }, CascadeMode.Stop);

    #endregion

    #region Format

    AddFailureForPropertyIf(
      nameof(EditEventRequest.Format),
      x => x == OperationType.Replace,
      new()
      {
        { x => Enum.TryParse(x.value?.ToString(), out FormatType _), "Incorrect format value." },
      });

    #endregion

    #region IsActive

    AddFailureForPropertyIf(
      nameof(EditEventRequest.IsActive),
      x => x == OperationType.Replace,
      new()
      {
        { x => bool.TryParse(x.value?.ToString(), out bool _), "Incorrect IsActive value." },
      });

    #endregion
  }

  public EditEventRequestValidator(IEventRepository repository)
  {
    RuleForEach(x => x.Item2.Operations)
      .Custom(HandleInternalPropertyValidation);

    RuleFor(request => request.Item1)
      .MustAsync((eventId, _) => repository.DoesExistAsync(eventId, null))
      .WithMessage("This event doesn't exist.");

    RuleFor(request => request.Item1)
      .MustAsync((eventId, _) => repository.IsEventCompletedAsync(eventId))
      .WithMessage("Can not edit completed event.");

    When(request => request.Item2.Operations.Any(
      o => o.path.Equals("/" + nameof(EditEventRequest.EndDate), StringComparison.OrdinalIgnoreCase)
      || o.path.Equals("/" + nameof(EditEventRequest.Date), StringComparison.OrdinalIgnoreCase)),
      () =>
      {
        RuleFor(request => request)
          .MustAsync(async (request, _) =>
          {
            DbEvent editedEvent = await repository.GetAsync(request.Item1);

            bool endDateOp = request.Item2.Operations.Any(
              o => o.path.Equals("/" + nameof(EditEventRequest.EndDate), StringComparison.OrdinalIgnoreCase));

            bool dateOp = request.Item2.Operations.Any(
              o => o.path.Equals("/" + nameof(EditEventRequest.Date), StringComparison.OrdinalIgnoreCase));

            DateTime? endDateValue;
            DateTime dateValue;

            if (endDateOp)
            {
              endDateValue = DateTime.TryParse(request.Item2.Operations.FirstOrDefault(
                x => x.path.Equals("/" + nameof(EditEventRequest.EndDate), StringComparison.OrdinalIgnoreCase))?.value?.ToString().Trim(),
                out DateTime endDate)
              ? endDate
              : null;
            }
            else
            {
              endDateValue = editedEvent.EndDate;
            }

            if (dateOp)
            {
              bool isParsedDate = DateTime.TryParse(request.Item2.Operations.FirstOrDefault(
                x => x.path.Equals("/" + nameof(EditEventRequest.Date), StringComparison.OrdinalIgnoreCase))?.value?.ToString().Trim(),
                out DateTime date);

              dateValue = date;
            }
            else
            {
              dateValue = editedEvent.Date;
            }

            return dateValue < endDateValue || endDateValue is null;
          })
          .WithMessage("The end date must be later than the event date.");
      });
  }
}
