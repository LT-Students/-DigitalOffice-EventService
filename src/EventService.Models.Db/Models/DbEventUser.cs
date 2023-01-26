using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using LT.DigitalOffice.EventService.Models.Db.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Models.Db.Models
{
  public record DbEventUser
  {
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public DbEvent Event { get; set; }
    public Guid UserId { get; set; }
    public Status Status { get; set; }
    public DateTime NotifiedAtUtc { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid ModifiedBy { get; set; }
    public DateTime ModifiedAtUtc { get; set; }
  }
  public class DbEventUserConfiguration : IEntityTypeConfiguration<DbEventUser>
  {
    public void Configure(EntityTypeBuilder<DbEventUser> builder)
    {
      builder
          .HasOne<DbEvent>(eu => eu.Event)
          .WithMany(e => e.EventUsers)
          .HasForeignKey(eu => eu.EventId);
    }
  }
}
