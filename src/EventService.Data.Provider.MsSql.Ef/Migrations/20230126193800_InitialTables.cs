using System;
using EventService.Data.Provider.MsSql.Ef;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.EventService.Data.Provider.MsSql.Ef.Migrations;

[DbContext(typeof(EventServiceDbContext))]
[Migration("20230126193800_InitialTables")]
public class InitialTables : Migration
{
  private void CreateEventsTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
    DbEvent.TableName,
    columns: table => new
    {
      Id = table.Column<Guid>(nullable: false),
      Name = table.Column<string>(nullable: false),
      Address = table.Column<string>(nullable: false),
      Description = table.Column<string>(nullable: true),
      Date = table.Column<DateTime>(nullable: false),
      Format = table.Column<int>(nullable: false),
      Access = table.Column<int>(nullable: false),
      IsActive = table.Column<bool>(nullable: false),
      CreatedBy = table.Column<Guid>(nullable: false),
      CreatedAtUtc = table.Column<DateTime>(nullable: false),
      ModifiedBy = table.Column<Guid>(nullable: true),
      ModifiedAtUtc = table.Column<DateTime>(nullable: true)
    },
    constraints: table =>
    {
      table.PrimaryKey($"PK_{DbEvent.TableName}", e => e.Id);
    });
  }

  private void CreateEventsCategoriesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
    DbEventCategory.TableName,
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
      table.PrimaryKey($"PK_{DbEventCategory.TableName}", ec => ec.Id);
    });
  }

  private void CreateCategoriesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
    DbCategory.TableName,
    columns: table => new
    {
      Id = table.Column<Guid>(nullable: false),
      Name = table.Column<string>(nullable: false),
      Color = table.Column<int>(nullable: false),
      IsActive = table.Column<bool>(nullable: false),
      CreatedBy = table.Column<Guid>(nullable: false),
      CreatedAtUtc = table.Column<DateTime>(nullable: false),
      ModifiedBy = table.Column<Guid>(nullable: true),
      ModifiedAtUtc = table.Column<DateTime>(nullable: true)
    },
    constraints: table =>
    {
      table.PrimaryKey($"PK_{DbCategory.TableName}", c => c.Id);
    });
  }

  private void CreateEventsUsersTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
    DbEventUser.TableName,
    columns: table => new
    {
      Id = table.Column<Guid>(nullable: false),
      EventId = table.Column<Guid>(nullable: false),
      UserId = table.Column<Guid>(nullable: false),
      Status = table.Column<int>(nullable: false),
      NotifyAtUtc = table.Column<DateTime>(nullable: true),
      CreatedBy = table.Column<Guid>(nullable: false),
      CreatedAtUtc = table.Column<DateTime>(nullable: false),
      ModifiedBy = table.Column<Guid>(nullable: true),
      ModifiedAtUtc = table.Column<DateTime>(nullable: true)
    },
    constraints: table =>
    {
      table.PrimaryKey($"PK_{DbEventUser.TableName}", eu => eu.Id);
    });
  }

  private void CreateEventsCommentsTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
    DbEventComment.TableName,
    columns: table => new
    {
      Id = table.Column<Guid>(nullable: false),
      Content = table.Column<string>(nullable: false),
      UserId = table.Column<Guid>(nullable: false),
      EventId = table.Column<Guid>(nullable: false),
      ParentId = table.Column<Guid>(nullable: true),
      IsActive = table.Column<bool>(nullable: false),
      CreatedAtUtc = table.Column<DateTime>(nullable: false),
      ModifiedBy = table.Column<Guid>(nullable: true),
      ModifiedAtUtc = table.Column<DateTime>(nullable: true)        
    },
    constraints: table =>
    {
      table.PrimaryKey($"PK_{DbEventComment.TableName}", ec => ec.Id);
    });
  }

  private void CreateEventsImagesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
    DbEventImage.TableName,
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
      table.PrimaryKey($"PK_{DbEventImage.TableName}", ei => ei.Id);
    });
  }

  private void CreateEventsFilesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
    DbEventFile.TableName,
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
      table.PrimaryKey($"PK_{DbEventFile.TableName}", ef => ef.Id);
    });
  }

  protected override void Up(MigrationBuilder migrationBuilder)
  {
    CreateEventsTable(migrationBuilder);
    CreateEventsCategoriesTable(migrationBuilder);
    CreateCategoriesTable(migrationBuilder);
    CreateEventsUsersTable(migrationBuilder);
    CreateEventsCommentsTable(migrationBuilder);
    CreateEventsImagesTable(migrationBuilder);
    CreateEventsFilesTable(migrationBuilder);
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(DbEvent.TableName);
    migrationBuilder.DropTable(DbEventCategory.TableName);
    migrationBuilder.DropTable(DbCategory.TableName);
    migrationBuilder.DropTable(DbEventUser.TableName);
    migrationBuilder.DropTable(DbEventComment.TableName);
    migrationBuilder.DropTable(DbEventImage.TableName);
    migrationBuilder.DropTable(DbEventFile.TableName);
  }
}
