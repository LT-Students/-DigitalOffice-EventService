using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LT.DigitalOffice.EventService.Models.Db;

public class DbUserBirthday
{
  public const string TableName = "UsersBirthdays";

  [Key]
  public Guid UserId { get; set; }
  public DateTime DateOfBirthday { get; set; }
  public bool IsActive { get; set; }
  public DateTime CreatedAtUtc { get; set; }
  public DateTime? ModifiedAtUtc { get; set; }
}

public class DbUserBirthdayConfiguration : IEntityTypeConfiguration<DbUserBirthday>
{
  public void Configure(EntityTypeBuilder<DbUserBirthday> builder)
  {
    builder
      .ToTable(DbUserBirthday.TableName);

    builder
      .HasKey(t => t.UserId);
  }
}
