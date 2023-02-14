using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces;

[AutoInject]
public interface ICategoryRepository
{
  public Task<bool> IsCategoryAddedAsync(Guid userId);
  public Task<Guid?> CreateAsync(DbCategory dbCategory);
}
