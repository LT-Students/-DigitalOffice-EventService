using System;
using LT.DigitalOffice.EventService.Models.Db;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.EventService.Data.Provider.MsSql.Ef.Migrations;

[DbContext(typeof(EventServiceDbContext))]
[Migration("20230605150000_InitialTables")]
public class InitialTables : Migration
{
  private void CreateEventsTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbEvent.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        Name = table.Column<string>(nullable: false),
        Address = table.Column<string>(nullable: true),
        Description = table.Column<string>(nullable: true),
        Date = table.Column<DateTime>(nullable: false),
        EndDate = table.Column<DateTime>(nullable: true),
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
      name: DbEventCategory.TableName,
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
      name: DbCategory.TableName,
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
      name: DbEventUser.TableName,
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

  private void CreateEventCommentsTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbEventComment.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        Content = table.Column<string>(nullable: true),
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

  private void CreateImagesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbImage.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        EntityId = table.Column<Guid>(nullable: false),
        ImageId = table.Column<Guid>(nullable: false)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbImage.TableName}", i => i.Id);
      });
  }

  private void CreateFilesTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbFile.TableName,
      columns: table => new
      {
        Id = table.Column<Guid>(nullable: false),
        EntityId = table.Column<Guid>(nullable: false),
        FileId = table.Column<Guid>(nullable: false)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbFile.TableName}", f => f.Id);
      });
  }

  private void CreateUsersBitrthdaysTable(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.CreateTable(
      name: DbUserBirthday.TableName,
      columns: table => new
      {
        UserId = table.Column<Guid>(nullable: false),
        DateOfBirth = table.Column<DateTime>(nullable: false),
        IsActive = table.Column<bool>(nullable: false),
        CreatedAtUtc = table.Column<DateTime>(nullable: false),
        ModifiedAtUtc = table.Column<DateTime>(nullable: true)
      },
      constraints: table =>
      {
        table.PrimaryKey($"PK_{DbUserBirthday.TableName}", ef => ef.UserId);
      });
  }

  protected override void Up(MigrationBuilder migrationBuilder)
  {
    CreateEventsTable(migrationBuilder);
    CreateEventsCategoriesTable(migrationBuilder);
    CreateCategoriesTable(migrationBuilder);
    CreateEventsUsersTable(migrationBuilder);
    CreateEventCommentsTable(migrationBuilder);
    CreateImagesTable(migrationBuilder);
    CreateFilesTable(migrationBuilder);
    CreateUsersBitrthdaysTable(migrationBuilder);
  }

  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(DbEvent.TableName);
    migrationBuilder.DropTable(DbEventCategory.TableName);
    migrationBuilder.DropTable(DbCategory.TableName);
    migrationBuilder.DropTable(DbEventUser.TableName);
    migrationBuilder.DropTable(DbEventComment.TableName);
    migrationBuilder.DropTable(DbImage.TableName);
    migrationBuilder.DropTable(DbFile.TableName);
  }
}
