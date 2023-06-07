using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.Kernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data.Provider;

[AutoInject(InjectType.Scoped)]
public interface IDataProvider : IBaseDataProvider
{
  public DbSet<DbEvent> Events { get; set; }
  public DbSet<DbCategory> Categories { get; set; }
  public DbSet<DbEventCategory> EventsCategories { get; set; }
  public DbSet<DbFile> Files { get; set; }
  public DbSet<DbImage> Images { get; set; }
  public DbSet<DbEventUser> EventsUsers { get; set; }
  public DbSet<DbEventComment> EventComments { get; set; }
  public DbSet<DbUserBirthday> UsersBirthdays { get; set; }
}
