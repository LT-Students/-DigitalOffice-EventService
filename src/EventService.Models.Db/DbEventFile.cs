using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automatonymous;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Models.Db
{
  public record DbEventFile
  {
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public DbEvent Event { get; set; }
    public Guid FileId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
  }
  public class DbEventFileConfiguration : IEntityTypeConfiguration<DbEventFile>
  {
    public void Configure(EntityTypeBuilder<DbEventFile> builder)
    {
      builder
          .HasOne(ef => ef.Event)
          .WithMany(e => e.EventFiles)
          .HasForeignKey(ef => ef.EventId);
    }
  }
}
