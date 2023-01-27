using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db
{
  public class DbEventImage
  {
    public const string TableName = "EventImage";

    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid ImageId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public DbEvent Event { get; set; }
  }

  public class DbEventImageConfiguration : IEntityTypeConfiguration<DbEventImage>
  {
    public void Configure(EntityTypeBuilder<DbEventImage> builder)
    {
      builder
        .ToTable(DbEventImage.TableName);

      builder
        .HasKey(t => t.Id);

      builder
        .HasOne(ei => ei.Event)
        .WithMany(e => e.EventImages);
    }
  }
}
