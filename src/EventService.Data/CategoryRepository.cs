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
  public async Task<bool> DoesExistAsync(List<Guid> categoryIds)
  {
    return await _provider.Categories.AsNoTracking().AnyAsync(c => categoryIds.Contains(c.Id) && c.IsActive);
  }
}
