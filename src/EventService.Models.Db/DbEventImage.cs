using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Models.Db
{
  public record DbEventImage
  {
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public DbEvent Event { get; set; }
    public Guid ImageId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }
  public class DbEventImageConfiguration : IEntityTypeConfiguration<DbEventImage>
  {
    public void Configure(EntityTypeBuilder<DbEventImage> builder)
    {
      builder
          .HasOne(ei => ei.Event)
          .WithMany(e => e.EventImages)
          .HasForeignKey(ei => ei.EventId);
    }
  }
}
