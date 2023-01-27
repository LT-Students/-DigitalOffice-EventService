using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using LT.DigitalOffice.EventService.Models.Db.Enums;
using LT.DigitalOffice.EventService.Models.Db.Models;

namespace LT.DigitalOffice.EventService.Models.Db
{
  public record DbCategory
  {
    public const string TableName = "Category";

    public Guid Id { get; set; }
    public string Name { get; set; }
    public Color Color { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public ICollection<DbEventCategory> EventCategories { get; set; }
  }
  public class DbCategoryConfiguration : IEntityTypeConfiguration<DbCategory>
  {
    public void Configure(EntityTypeBuilder<DbCategory> builder)
    {
      builder
        .HasMany(e => e.EventCategories)
        .WithOne(ec => ec.Category)
        .HasForeignKey(ec => ec.CategoryId);
    }
  }
}
