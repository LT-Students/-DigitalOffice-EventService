using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db;

public class DbFile
{
  public const string TableName = "Files";

  public Guid Id { get; set; }
  public Guid EntityId { get; set; }
  public Guid FileId { get; set; }

  public DbEvent Event { get; set; }
  public DbEventComment EventComment { get; set; }
}

public class DbFileConfiguration : IEntityTypeConfiguration<DbFile>
{
  public void Configure(EntityTypeBuilder<DbFile> builder)
  {
    builder
      .ToTable(DbFile.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(ef => ef.Event)
      .WithMany(e => e.Files)
      .HasForeignKey(ef => ef.EntityId);

    builder
      .HasOne(cf => cf.EventComment)
      .WithMany(c => c.Files)
      .HasForeignKey(cf => cf.EntityId);
  }
}
