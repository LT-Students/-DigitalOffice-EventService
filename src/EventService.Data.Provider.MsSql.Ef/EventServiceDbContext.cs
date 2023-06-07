using System.Reflection;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Data.Provider.MsSql.Ef;

public class EventServiceDbContext : DbContext, IDataProvider
{
  public DbSet<DbEvent> Events { get; set; }
  public DbSet<DbCategory> Categories { get; set; }
  public DbSet<DbEventCategory> EventsCategories { get; set; }
  public DbSet<DbFile> Files { get; set; }
  public DbSet<DbImage> Images { get; set; }
  public DbSet<DbEventUser> EventsUsers { get; set; }
  public DbSet<DbEventComment> EventComments { get; set; }
  public DbSet<DbUserBirthday> UsersBirthdays { get; set; }

  public EventServiceDbContext(DbContextOptions<EventServiceDbContext> options)
    : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("LT.DigitalOffice.EventService.Models.Db"));
  }

  public void Save()
  {
    SaveChanges();
  }

  public async Task SaveAsync()
  {
    await SaveChangesAsync();
  }

  public object MakeEntityDetached(object obj)
  {
    Entry(obj).State = EntityState.Detached;

    return Entry(obj).State;
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
