using System.Reflection;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using LT.DigitalOffice.Kernel.EFSupport.Provider;
using Microsoft.EntityFrameworkCore;

namespace EventService.Data.Provider.MsSql.Ef;

public class EventServiceDbContext : DbContext, IDataProvider
{
  public DbSet<DbEvent> Events { get; set; }
  public DbSet<DbCategory> Categories { get; set; }
  public DbSet<DbEventCategory> EventsCategories { get; set; }
  public DbSet<DbEventFile> EventFiles { get; set; }
  public DbSet<DbEventImage> EventImages { get; set; }
  public DbSet<DbEventComment> EventComments { get; set; }
  public DbSet<DbEventUser> EventsUsers { get; set; }

  public EventServiceDbContext(DbContextOptions<EventServiceDbContext> options)
    : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("LT.DigitalOffice.EventService.Models.Db"));
  }

  public object MakeEntityDetached(object obj)
  {
    Entry(obj).State = EntityState.Detached;
    return Entry(obj).State;
  }

  async Task IBaseDataProvider.SaveAsync()
  {
    await SaveChangesAsync();
  }

  void IBaseDataProvider.Save()
  {
    SaveChanges();
  }

  public void EnsureDeleted()
  {
    Database.EnsureDeleted();
  }

  public bool IsInMemory()
  {
    return Database.IsInMemory();
  }
}
