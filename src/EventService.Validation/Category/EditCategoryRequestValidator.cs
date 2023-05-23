using System;
using System.Collections.Generic;
using FluentValidation;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.EventService.Validation.Category.Interfaces;
using LT.DigitalOffice.EventService.Validation.EventUser.Resources;
using LT.DigitalOffice.Kernel.Validators;
using MassTransit.Contracts.Conductor;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace LT.DigitalOffice.EventService.Validation.Category
{
  public class EditCategoryRequestValidator : ExtendedEditRequestValidator<Guid, EditCategoryRequest>, IEditCategoryRequestValidator
  {
    private void HandleInternalPropertyValidation(
      Operation<EditCategoryRequest> requestedOperation,
      ValidationContext<(Guid, JsonPatchDocument<EditCategoryRequest>)> context)
    {
      Context = context;
      RequestedOperation = requestedOperation;

      #region paths

      AddСorrectPaths(
        new List<string>
        {
        nameof(EditCategoryRequest.Color),
        nameof(EditCategoryRequest.Name),
        });

      AddСorrectOperations(nameof(EditCategoryRequest.Color), new List<OperationType> { OperationType.Replace });
      AddСorrectOperations(nameof(EditCategoryRequest.Name), new List<OperationType> { OperationType.Replace });

      #endregion

      #region Color

      AddFailureForPropertyIf(
        nameof(EditCategoryRequest.Color),
        x => x == OperationType.Replace,
        new()
        {
          { x => Enum.TryParse(x.value?.ToString(), out CategoryColor _), "Incorrect Color value." }
        });

      #endregion

      #region IsActive

      AddFailureForPropertyIf(
        nameof(EditCategoryRequest.Name),
        x => x == OperationType.Replace,
        new()
        {
          { x => !string.IsNullOrEmpty(x.value?.ToString().Trim()), "Name must not be empty." },
          { x => x.value?.ToString().Length < 21, "Name is too long." }
        }, CascadeMode.Stop);

      #endregion
    }

    public EditCategoryRequestValidator(
      ICategoryRepository categoryRepository)
    {
      RuleForEach(x => x.Item2.Operations)
        .Custom(HandleInternalPropertyValidation);

      RuleFor(categoryId => categoryId.Item1)
        .MustAsync(async (categoryId, _) => await categoryRepository.DoesExistAllAsync(new List<Guid> { categoryId }))
        .WithMessage("This Id doesn't exist.");
    }
  }
}
