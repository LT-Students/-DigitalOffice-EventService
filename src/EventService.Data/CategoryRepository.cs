using System;
using System.Linq;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;

namespace LT.DigitalOffice.EventService.Data;

public class CategoryRepository : ICategoryRepository
{
  private readonly IDataProvider _provider;

  private IQueryable<DbCategory> CreateFindPredicates(
  FindCategoriesFilter filter)
  {
    IQueryable<DbCategory> dbCategories = _provider.Categories.AsNoTracking();

    if (!string.IsNullOrWhiteSpace(filter.NameIncludeSubstring))
    {
      dbCategories = dbCategories.Where(c => c.Name.Contains(filter.NameIncludeSubstring));
    }

    if (filter.Color.HasValue)
    {
      dbCategories = dbCategories.Where(c => c.Color == filter.Color);
    }

    if (!filter.IncludeDeactivated)
    {
      dbCategories = dbCategories.Where(c => c.IsActive);
    }

    return dbCategories;
  }
  
  public CategoryRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public bool DoesExistAllAsync(List<Guid> categoriesIds)
  {
    return categoriesIds.All(categoryId =>
      _provider.Categories.AsNoTracking().AnyAsync(c => c.Id == categoryId && c.IsActive).Result);
  }

  public async Task<Guid?> CreateAsync(DbCategory dbCategory)
  {
    if (dbCategory is null)
    {
      return null;
    }

    _provider.Categories.Add(dbCategory);
    await _provider.SaveAsync();

    return dbCategory.Id;
  }

  public async Task<(List<DbCategory> dbCategories, int totalCount)> FindAsync(
    FindCategoriesFilter filter, 
    CancellationToken cancellationToken = default)
  {
    if (filter is null)
    {
      return default;
    }

    IQueryable<DbCategory> dbCategories = CreateFindPredicates(filter);

    return (
      await dbCategories
        .Skip(filter.SkipCount)
        .Take(filter.TakeCount)
        .ToListAsync(cancellationToken),
      await dbCategories.CountAsync(cancellationToken));
  }
}

