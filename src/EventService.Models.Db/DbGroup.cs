using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db;

public class DbGroup
{
  public const string TableName = "Groups";

  public Guid Id { get; set; }
  public string Name { get; set; }
  public int? MaxGuests { get; set; }
  public string Description { get; set; }
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }

  public ICollection<DbGroupEventUser> GroupsEventsUsers { get; set; }
  public ICollection<DbOccasionGroup> OccasionsGroups { get; set; }
}

public class DbGroupConfiguration : IEntityTypeConfiguration<DbGroup>
{
  public void Configure(EntityTypeBuilder<DbGroup> builder)
  {
    builder
      .ToTable(DbCategory.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasMany(e => e.GroupsEventsUsers)
      .WithOne(ec => ec.Group);
    
    builder
      .HasMany(e => e.OccasionsGroups)
      .WithOne(ec => ec.Group);
  }
}
