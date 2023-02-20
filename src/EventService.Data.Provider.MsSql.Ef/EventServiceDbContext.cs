using System;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Data.Provider;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore;

namespace EventService.Data.Provider.MsSql.Ef
{
  public class EventServiceDbContext : DbContext, IDataProvider
  {
    public EventServiceDbContext(DbContextOptions<EventServiceDbContext> options)
      : base(options)
    {
    }

    public DbSet<DbEvent> Events { get; set; }
    public DbSet<DbCategory> Categories { get; set; }
    public DbSet<DbEventCategory> EventsCategories { get; set; }
    public DbSet<DbEventFile> EventFiles { get; set; }
    public DbSet<DbEventImage> EventImages { get; set; }
    public DbSet<DbEventUser> EventsUsers { get; set; }
    public DbSet<DbEventComment> EventComments { get; set; }

    public void Save()
    {
      throw new NotImplementedException();
    }

    public async Task SaveAsync()
    {
      await SaveChangesAsync();
    }

    public object MakeEntityDetached(object obj)
    {
      throw new NotImplementedException();
    }

    public void EnsureDeleted()
    {
      throw new NotImplementedException();
    }

    public bool IsInMemory()
    {
      throw new NotImplementedException();
    }
  }
}
