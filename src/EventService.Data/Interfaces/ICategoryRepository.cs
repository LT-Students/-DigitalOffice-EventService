using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.EventService.Data.Interfaces
{
  [AutoInject] 
  public interface ICategoryRepository
  {
    Task<bool> IsCategoryExist(Guid eventId);
    Task<Guid?> CreateAsync(DbCategory dbCategory);
  }
}
