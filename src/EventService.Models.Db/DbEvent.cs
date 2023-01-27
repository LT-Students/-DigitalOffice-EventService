using System;
using System.Collections.Generic;
using LT.DigitalOffice.EventService.Models.Dto.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db
{
  public class DbEvent
  {
    public const string TableName = "Event";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public FormatType Format { get; set; }
    public AccessType Access { get; set; }
    public bool IsActive { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? ModifiedAtUtc { get; set; }

    public ICollection<DbEventCategory> EventCategories { get; set; }
    public ICollection<DbEventFile> EventFiles { get; set; }
    public ICollection<DbEventImage> EventImages { get; set; }
    public ICollection<DbEventUser> EventUsers { get; set; }
    public ICollection<DbEventComment> EventComments { get; set; }

    public DbEvent()
    {
      EventCategories = new HashSet<DbEventCategory>();
      EventFiles = new HashSet<DbEventFile>();
      EventImages = new HashSet<DbEventImage>();
      EventUsers = new HashSet<DbEventUser>();
      EventComments = new HashSet<DbEventComment>();
    }

    public class DbEventConfiguration : IEntityTypeConfiguration<DbEvent>
    {
      public void Configure(EntityTypeBuilder<DbEvent> builder)
      {
        builder
          .ToTable(DbEvent.TableName);

        builder
          .HasKey(t => t.Id);

        builder
          .HasMany(e => e.EventCategories)
          .WithOne(ec => ec.Event);

        builder
          .HasMany(e => e.EventFiles)
          .WithOne(ef => ef.Event);

        builder
          .HasMany(e => e.EventImages)
          .WithOne(ei => ei.Event);

        builder
          .HasMany(e => e.EventUsers)
          .WithOne(eu => eu.Event);
      }
    }
  }
}
