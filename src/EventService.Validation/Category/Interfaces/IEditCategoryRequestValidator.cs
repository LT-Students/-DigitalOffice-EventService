using System;
using FluentValidation;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.Kernel.Attributes;
using Microsoft.AspNetCore.JsonPatch;

namespace LT.DigitalOffice.EventService.Validation.Category.Interfaces;

[AutoInject]
public interface IEditCategoryRequestValidator : IValidator<(Guid, JsonPatchDocument<EditCategoryRequest>)>
{
}
