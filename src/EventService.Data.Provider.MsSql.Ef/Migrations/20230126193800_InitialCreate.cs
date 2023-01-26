using System;
using EventService.Data.Provider.MsSql.Ef;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.EventService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(EventServiceDbContext))]
  [Migration("20230126193800_InitialCreate")]
  public class _20230126193800_InitialCreate : Migration
  {
    private const string _KeyPrefix = "PK_";
    private const string _EventTableName = "Events";
    private const string _EventCategoryTableName = "EventsCategories";
    private const string _CategoryTableName = "Categories";
    private const string _EventUserTableName = "EventsUsers";
    private const string _CommentTableName = "Comments";
    private const string _EventImageTableName = "EventsImages";
    private const string _EventFileTableName = "EventsFiles";
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
      name: $"{_EventTableName}",
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        Name = table.Column<string>(nullable: false),
        Address = table.Column<string>(nullable: false),
        Description = table.Column<string>(nullable: true),
        Date = table.Column<DateTime>(nullable: false),
        FormatType = table.Column<int>(nullable: false),
        AccessType = table.Column<int>(nullable: false),
        IsActive = table.Column<bool>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },

      constraints: table =>
      {
        table.PrimaryKey($"{_KeyPrefix}{_EventTableName}", e => e.Id);
      });

      migrationBuilder.CreateTable(
      name: $"{_EventCategoryTableName}",
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        EventId = table.Column<Guid>(nullable: false),
        CategoryId = table.Column<Guid>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false)
      },

      constraints: table =>
      {
        table.PrimaryKey($"{_KeyPrefix}{_EventCategoryTableName}", ec => ec.Id);
      });

      migrationBuilder.CreateTable(
      name: $"{_CategoryTableName}",
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        Name = table.Column<string>(nullable: false),
        Color = table.Column<int>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        IsActive = table.Column<bool>(nullable: false)
      },

      constraints: table =>
      {
        table.PrimaryKey($"{_KeyPrefix}{_CategoryTableName}", с => с.Id);
      });

      migrationBuilder.CreateTable(
      name: $"{_EventUserTableName}",
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        EventId = table.Column<Guid>(nullable: false),
        UserId = table.Column<Guid>(nullable: false),
        Status = table.Column<int>(nullable: false),
        NotifiedAtUtc = table.Column<DateTime>(nullable: true),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },

      constraints: table =>
      {
        table.PrimaryKey($"{_KeyPrefix}{_EventUserTableName}", eu => eu.Id);
      });

      migrationBuilder.CreateTable(
      name: $"{_CommentTableName}",
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        UserId = table.Column<Guid>(nullable: false),
        EventId = table.Column<Guid>(nullable: false),
        ParentComment = table.Column<Guid>(nullable: true),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedBy = table.Column<Guid>(nullable: true),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true),
        IsActive = table.Column<bool>(nullable: false)
      },

      constraints: table =>
      {
        table.PrimaryKey($"{_KeyPrefix}{_CommentTableName}", com => com.Id);
      });

      migrationBuilder.CreateTable(
      name: $"{_EventImageTableName}",
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        EventId = table.Column<Guid>(nullable: false),
        ImageId = table.Column<Guid>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
      },

      constraints: table =>
      {
        table.PrimaryKey($"{_KeyPrefix}{_EventImageTableName}", ei => ei.Id);
      });

      migrationBuilder.CreateTable(
      name: $"{_EventFileTableName}",
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        EventId = table.Column<Guid>(nullable: false),
        FileId = table.Column<Guid>(nullable: false),
        CreatedBy = table.Column<Guid>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
      },

      constraints: table =>
      {
        table.PrimaryKey($"{_KeyPrefix}{_EventFileTableName}", ef => ef.Id);
      });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(_EventTableName);
      migrationBuilder.DropTable(_EventCategoryTableName);
      migrationBuilder.DropTable(_CategoryTableName);
      migrationBuilder.DropTable(_CommentTableName);
      migrationBuilder.DropTable(_EventImageTableName);
      migrationBuilder.DropTable(_EventFileTableName);
    }
  }
}
