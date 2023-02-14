using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data;

public class CategoryRepository : ICategoryRepository
{
  private readonly IDataProvider _provider;

  public CategoryRepository(
      IDataProvider provider)
  {
    _provider = provider;
  }

  public async Task<bool> DoesExistAsync(Guid categoryId)
  {
    return await _provider.Categories.AnyAsync(c => c.Id == categoryId && c.IsActive);
  }
}
