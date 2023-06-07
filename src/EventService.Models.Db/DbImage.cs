using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db;

public class DbImage
{
  public const string TableName = "Images";

  public Guid Id { get; set; }
  public Guid EntityId { get; set; }
  public Guid ImageId { get; set; }

  public DbEvent Event { get; set; }
  public DbEventComment EventComment { get; set; }
}

public class DbImageConfiguration : IEntityTypeConfiguration<DbImage>
{
  public void Configure(EntityTypeBuilder<DbImage> builder)
  {
    builder
      .ToTable(DbImage.TableName);

    builder
      .HasKey(i => i.Id);

    builder
      .HasOne(ei => ei.Event)
      .WithMany(e => e.Images)
      .HasForeignKey(ei => ei.EntityId);

    builder
      .HasOne(ci => ci.EventComment)
      .WithMany(c => c.Images)
      .HasForeignKey(ci => ci.EntityId);
  }
}
