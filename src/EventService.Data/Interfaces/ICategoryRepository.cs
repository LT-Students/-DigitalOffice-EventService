using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface ICategoryRepository
{
  bool DoesExistAllAsync(List<Guid> categoriesIds);
  Task<Guid?> CreateAsync(DbCategory dbCategory);
  Task<(List<DbCategory> dbCategories, int totalCount)> FindAsync(
    FindCategoriesFilter filter, 
    CancellationToken cancellationToken);
}
