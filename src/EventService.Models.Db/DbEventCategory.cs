using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Models.Db
{
  public record DbEventCategory
  {
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public DbEvent Event { get; set; }
    public Guid CategoryId { get; set; }
    public DbCategory Category { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }
  public class DbEventCategoryConfiguration : IEntityTypeConfiguration<DbEventCategory>
  {
    public void Configure(EntityTypeBuilder<DbEventCategory> builder)
    {
      builder
          .HasOne(ec => ec.Event)
          .WithMany(e => e.EventCategories)
          .HasForeignKey(ec => ec.EventId);

      builder
          .HasOne(ec => ec.Category)
          .WithMany(c => c.EventCategories)
          .HasForeignKey(ec => ec.CategoryId);
    }
  }
}
