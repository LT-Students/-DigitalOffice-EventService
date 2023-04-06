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

  private async Task<IQueryable<DbCategory>> CreateFindPredicates(
  FindCategoriesFilter filter,
  Guid categoryId)
  {
    IQueryable<DbCategory> dbCategories = _provider.Categories.AsNoTracking().Where(c => c.Id == categoryId);

    if (!string.IsNullOrWhiteSpace(filter.UserFullNameIncludeSubstring))
    {
      dbCategories = dbCategories.Where(c => c.Name.Contains(filter.UserFullNameIncludeSubstring));
    }

    if (filter.Color.HasValue)
    {
      dbCategories = dbCategories.Where(c => c.Color == filter.Color);
    }
    
    if (filter.IsActive.HasValue)
    {
      dbCategories = dbCategories.Where(c => c.IsActive == filter.IsActive);
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
    Guid categoryId, 
    FindCategoriesFilter filter, 
    CancellationToken cancellationToken = default)
  {
    if (filter is null)
    {
      return default;
    }

    IQueryable<DbCategory> dbCategories = await CreateFindPredicates(filter, categoryId);

    return (
      await dbCategories
        .Skip(filter.SkipCount)
        .Take(filter.TakeCount)
        .ToListAsync(cancellationToken),
      await dbCategories.CountAsync(cancellationToken));
  }
}

