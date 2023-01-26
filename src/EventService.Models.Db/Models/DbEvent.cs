using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.EventService.Models.Db.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace LT.DigitalOffice.EventService.Models.Db.Models
{
  public record DbEvent
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public FormatType FormatType { get; set; }
    public AccessType AccessType { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid ModifiedBy { get; set; }
    public DateTime ModifiedAtUtc { get; set; }
    public ICollection<DbEventCategory> EventCategories { get; set; }
    public ICollection<DbEventFile> EventFiles { get; set; }
    public ICollection<DbEventImage> EventImages { get; set; }
    public ICollection<DbEventUser> EventUsers { get; set; }
    public ICollection<DbComment> EventComments { get; set; }

    public DbEvent()
    {
      EventCategories = new HashSet<DbEventCategory>();
      EventFiles = new HashSet<DbEventFile>();
      EventImages = new HashSet<DbEventImage>();
      EventUsers = new HashSet<DbEventUser>();
      EventComments = new HashSet<DbComment>();
    }
    public class DbEventConfiguration : IEntityTypeConfiguration<DbEvent>
    {
      public void Configure(EntityTypeBuilder<DbEvent> builder)
      {
        builder
            .HasMany<DbEventCategory>(e => e.EventCategories)
            .WithOne(ec => ec.Event)
            .HasForeignKey(ec => ec.EventId);

        builder
            .HasMany<DbEventFile>(e => e.EventFiles)
            .WithOne(ef => ef.Event)
            .HasForeignKey(ef => ef.EventId);

        builder
            .HasMany<DbEventImage>(e => e.EventImages)
            .WithOne(ei => ei.Event)
            .HasForeignKey(ei => ei.EventId);

        builder
            .HasMany<DbEventUser>(e => e.EventUsers)
            .WithOne(eu => eu.Event)
            .HasForeignKey(eu => eu.EventId);
      }
    }
  }
}
