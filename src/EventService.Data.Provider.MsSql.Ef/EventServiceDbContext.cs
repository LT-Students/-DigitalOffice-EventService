using LT.DigitalOffice.EventService.Models.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace EventService.Data.Provider.MsSql.Ef
{
  public class EventServiceDbContext : DbContext
  {
    public DbSet<DbEvent> Events { get; set; }
    public DbSet<DbCategory> Categories { get; set; }
    public DbSet<DbEventCategory> EventCategories { get; set; }
    public DbSet<DbEventFile> EventFiles { get; set; }
    public DbSet<DbEventImage> EventImages { get; set; }
    public DbSet<DbEventUser> EventUsers { get; set; }
    public DbSet<DbComment> EventComments { get; set; }
    public EventServiceDbContext(DbContextOptions<EventServiceDbContext> options)
      : base(options)
    {
    }
  }
}
