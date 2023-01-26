using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Models.Db.Models
{
  public record DbComment
  {
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public DbEvent Event { get; set; }
    public Guid? ParentComment { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid ModifiedBy { get; set; }
    public DateTime ModifiedAtUtc { get; set; }
    public bool IsActive { get; set; }
  }
  public class DbEventCommentsConfiguration : IEntityTypeConfiguration<DbComment>
  {
    public void Configure(EntityTypeBuilder<DbComment> builder)
    {
      builder
          .HasOne<DbEvent>(e => e.Event)
          .WithMany(ec => ec.EventComments)
          .HasForeignKey(ec => ec.EventId);

    }
  }
}
