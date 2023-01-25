using Microsoft.EntityFrameworkCore;

namespace EventService.Data.Provider.MsSql.Ef
{
  public class EventServiceDbContext : DbContext
  {
    public EventServiceDbContext(DbContextOptions<EventServiceDbContext> options)
      : base(options)
    {
    }
  }
}