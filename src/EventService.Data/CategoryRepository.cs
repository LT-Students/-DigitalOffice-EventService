using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

  public Task<bool> DoesExistAsync(List<Guid> categoryId)
  {
    return _provider.Categories.AsNoTracking().AnyAsync(c => categoryId.Contains(c.Id) && c.IsActive);
  }
}
