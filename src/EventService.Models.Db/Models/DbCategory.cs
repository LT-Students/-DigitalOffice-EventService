using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using LT.DigitalOffice.EventService.Models.Db.Enums;

namespace LT.DigitalOffice.EventService.Models.Db.Models
{
  public record DbCategory
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }
    public Guid Createdby { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public ICollection<DbEventCategory> EventCategories { get; set; }

  }
  public class DbCategoryConfiguration : IEntityTypeConfiguration<DbCategory>
  {
    public void Configure(EntityTypeBuilder<DbCategory> builder)
    {
      builder
          .HasMany<DbEventCategory>(e => e.EventCategories)
          .WithOne(ec => ec.Category)
          .HasForeignKey(ec => ec.CategoryId);
    }
  }
}
