using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db
{
  public class DbEventFile
  {
    public const string TableName = "EventFile";

    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid FileId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }

    public DbEvent Event { get; set; }
  }

  public class DbEventFileConfiguration : IEntityTypeConfiguration<DbEventFile>
  {
    public void Configure(EntityTypeBuilder<DbEventFile> builder)
    {
      builder
        .ToTable(DbEventFile.TableName);

      builder
        .HasKey(t => t.Id);

      builder
        .HasOne(ef => ef.Event)
        .WithMany(e => e.EventFiles);
    }
  }
}
