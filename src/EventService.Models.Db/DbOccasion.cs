using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db;

public class DbOccasion
{
  public const string TableName = "Occasions";
  public Guid Id { get; set; }
  public Guid EventId { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public int? MaxGroup { get; set; } 
  public bool IsActive { get; set; }
  public Guid CreatedBy { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }
  
  public DbEvent Event { get; set; }
  public ICollection<DbOccasionGroup> OccasionsGroups { get; set; }
  
  public class DbOccasionConfiguration : IEntityTypeConfiguration<DbOccasion>
  {
    public void Configure(EntityTypeBuilder<DbOccasion> builder)
    {
      builder
        .ToTable(TableName);

      builder
        .HasKey(t => t.Id);

      builder
        .HasOne(eu => eu.Event)
        .WithMany(e => e.Occasions);
      
      builder
        .HasMany(e => e.OccasionsGroups)
        .WithOne(ec => ec.Occasion);
    }
  }
}
