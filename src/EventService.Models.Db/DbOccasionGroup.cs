using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db;

public class DbOccasionGroup
{
  public const string TableName = "OccasionGroups";

  public Guid Id { get; set; }
  public Guid GroupId { get; set; }
  public Guid OccasionId { get; set; }
  public Guid CreatedBy { get; set; }
  
  public DbGroup Group { get; set; }
  public DbOccasion Occasion { get; set; }
}

public class DbOccasionGroupConfiguration : IEntityTypeConfiguration<DbOccasionGroup>
{
  public void Configure(EntityTypeBuilder<DbOccasionGroup> builder)
  {
    builder
      .ToTable(DbEventCategory.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(ec => ec.Group)
      .WithMany(e => e.OccasionsGroups);

    builder
      .HasOne(ec => ec.Occasion)
      .WithMany(c => c.OccasionsGroups);
  }
}

