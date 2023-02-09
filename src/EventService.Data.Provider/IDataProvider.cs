using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using LT.DigitalOffice.Kernel.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventService.Data.Provider;

[AutoInject(InjectType.Scoped)]
public interface IDataProvider : IBaseDataProvider
{
  public DbSet<DbEvent> Events { get; set; }
  public DbSet<DbCategory> Categories { get; set; }
  public DbSet<DbEventCategory> EventsCategories { get; set; }
  public DbSet<DbEventFile> EventFiles { get; set; }
  public DbSet<DbEventImage> EventImages { get; set; }
  public DbSet<DbEventUser> EventsUsers { get; set; }
  public DbSet<DbEventComment> EventComments { get; set; }
}
