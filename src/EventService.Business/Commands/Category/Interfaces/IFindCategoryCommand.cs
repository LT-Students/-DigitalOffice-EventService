using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Dto.Models;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Responses;

namespace LT.DigitalOffice.EventService.Business.Commands.Category.Interfaces;

[AutoInject]
public interface IFindCategoryCommand
{
  Task<FindResultResponse<List<CategoryInfo>>> ExecuteAsync(
    Guid eventId,
    FindCategoriesFilter filter,
    CancellationToken cancellationToken = default);
}

