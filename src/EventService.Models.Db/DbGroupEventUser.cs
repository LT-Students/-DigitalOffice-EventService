using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db;

public class DbGroupEventUser
{
  public const string TableName = "GroupsEventsUsers";

  public Guid Id { get; set; }
  public Guid EventUserId { get; set; }
  public Guid GroupId { get; set; }
  public Guid CreatedBy { get; set; }

  public DbEventUser EventUser { get; set; }
  public DbGroup Group { get; set; }
}

public class DbGroupEventUserConfiguration : IEntityTypeConfiguration<DbGroupEventUser>
{
  public void Configure(EntityTypeBuilder<DbGroupEventUser> builder)
  {
    builder
      .ToTable(DbEventCategory.TableName);

    builder
      .HasKey(t => t.Id);

    builder
      .HasOne(ec => ec.EventUser)
      .WithMany(e => e.GroupsEventsUsers);

    builder
      .HasOne(ec => ec.Group)
      .WithMany(c => c.GroupsEventsUsers);
  }
}

