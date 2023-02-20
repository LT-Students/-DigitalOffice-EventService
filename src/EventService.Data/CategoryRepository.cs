using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Interfaces;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data
{
  public class CategoryRepository : ICategoryRepository
  {
    private readonly IDataProvider _provider;

    public CategoryRepository(IDataProvider provider)
    {
      _provider = provider;
    }

    public async Task<bool> IsCategoryExist(Guid eventId)
    { 
      return await _provider.Categories.AnyAsync(e => e.Id == eventId);
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
  }
}
