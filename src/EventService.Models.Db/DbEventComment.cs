using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db
{
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

    public DbEvent Event { get; set; }
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
    }
  }
}
