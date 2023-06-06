using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db;

public class DbEventComment
{
  public const string TableName = "EventComments";

  public Guid Id { get; set; }
  public string Content { get; set; }
  public Guid UserId { get; set; }
  public Guid EventId { get; set; }
  public Guid? ParentId { get; set; }
  public bool IsActive { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public Guid? ModifiedBy { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }
  public ICollection<DbFile> Files { get; set; }
  public ICollection<DbImage> Images { get; set; }

  public DbEvent Event { get; set; }

  public DbEventComment()
  {
    Files = new HashSet<DbFile>();
    Images = new HashSet<DbImage>();
  }
}

public class DbEventCommentsConfiguration : IEntityTypeConfiguration<DbEventComment>
{
  public void Configure(EntityTypeBuilder<DbEventComment> builder)
  {
    builder
      .ToTable(DbEventComment.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(e => e.Event)
      .WithMany(ec => ec.Comments);

    builder
      .HasMany(ec => ec.Images)
      .WithOne(i => i.EventComment)
      .HasForeignKey(i => i.EntityId);

    builder
      .HasMany(ec => ec.Files)
      .WithOne(f => f.EventComment);
  }
}
