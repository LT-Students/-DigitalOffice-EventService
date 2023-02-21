using System;
using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data;

public class CategoryRepository : ICategoryRepository
{
  private readonly IDataProvider _provider;

  public CategoryRepository(IDataProvider provider)
  {
    _provider = provider;
  }

  public bool DoesExistAllAsync(List<Guid> categoryIds)
  {
    return categoryIds.All(categoryId =>
      _provider.Categories.AsNoTracking().AnyAsync(c => c.Id == categoryId && c.IsActive).Result);
  }
}
