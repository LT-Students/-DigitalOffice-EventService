﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using LT.DigitalOffice.EventService.Models.Dto.Requests.Category;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LT.DigitalOffice.EventService.Data;

public class CategoryRepository : ICategoryRepository
{
  private readonly IDataProvider _provider;
  private readonly IHttpContextAccessor _httpContextAccessor;

  private IQueryable<DbCategory> CreateFindPredicates(
    FindCategoriesFilter filter)
  {
    IQueryable<DbCategory> dbCategories = _provider.Categories.AsNoTracking().Where(c => c.IsActive);

    if (!string.IsNullOrWhiteSpace(filter.NameIncludeSubstring))
    {
      dbCategories = dbCategories.Where(c => c.Name.Contains(filter.NameIncludeSubstring));
    }

    if (filter.Color.HasValue)
    {
      dbCategories = dbCategories.Where(c => c.Color == filter.Color);
    }

    if (filter.IsAscendingSort.HasValue)
    {
      dbCategories = filter.IsAscendingSort.Value
        ? dbCategories.OrderBy(c => c.Name)
        : dbCategories.OrderByDescending(c => c.Name);
    }

    return dbCategories;
  }

  public CategoryRepository(
    IDataProvider provider,
    IHttpContextAccessor httpContextAccessor)
  {
    _provider = provider;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<bool> DoExistAllAsync(List<Guid> categoriesIds)
  {
    return (await _provider.Categories
      .Where(p => categoriesIds.Contains(p.Id) && p.IsActive)
      .Select(p => p.Id)
      .ToListAsync()).Count == categoriesIds.Count;
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

  public Task CreateAsync(List<DbCategory> dbCategories)
  {
    if (dbCategories.IsNullOrEmpty())
    {
      return null;
    }

    _provider.Categories.AddRange(dbCategories);

    return _provider.SaveAsync();
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

  public async Task<bool> EditAsync(Guid categoryId, JsonPatchDocument<DbCategory> request)
  {
    DbCategory dbCategory = await _provider.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

    if (dbCategory is null || request is null)
    {
      return false;
    }

    bool isActive = dbCategory.IsActive;

    request.ApplyTo(dbCategory);
    dbCategory.ModifiedBy = _httpContextAccessor.HttpContext.GetUserId();
    dbCategory.ModifiedAtUtc = DateTime.UtcNow;

    if (isActive != dbCategory.IsActive)
    {
      _provider.EventsCategories.RemoveRange(
        _provider.EventsCategories.Where(ec => ec.CategoryId == dbCategory.Id));
    }

    await _provider.SaveAsync();

    return true;
  }

  public Task<bool> DoesExistAsync(string name, CategoryColor color)
  {
    return _provider.Categories
      .Where(c => c.Name == name && c.Color == color && c.IsActive)
      .AnyAsync();
  }
}

