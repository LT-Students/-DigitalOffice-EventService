using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventService.Data.Provider.MsSql.Ef;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LT.DigitalOffice.EventService.Data.Provider.MsSql.Ef.Migrations
{
  [DbContext(typeof(EventServiceDbContext))]
  [Migration("20230126193800_InitialCreated")]

  public class _20230126193800_InitialCreated : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {

      migrationBuilder.CreateTable(

          name: "Event",

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
            table.PrimaryKey("PK_Event", e => e.Id);
          });

      migrationBuilder.CreateTable(

          name: "EventCategory",

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
            table.PrimaryKey("PK_EventCategory", ec => ec.Id);
          });

      migrationBuilder.CreateTable(

          name: "Category",

    columns: table => new
    {
      Id = table.Column<Guid>(nullable: false),
      Name = table.Column<string>(nullable: false),
      Color = table.Column<int>(nullable: false),
      CreatedBy = table.Column<Guid>(nullable: false),
      CreatedAtUtc = table.Column<DateTime>(nullable: false)
    },

    constraints: table =>
    {
      table.PrimaryKey("PK_Category", с => с.Id);
    });

      migrationBuilder.CreateTable(

          name: "EventUser",

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
            table.PrimaryKey("PK_EventUser", eu => eu.Id);
          });

      migrationBuilder.CreateTable(

          name: "Comment",

          columns: table => new
          {
            Id = table.Column<Guid>(nullable: false),
            UserId = table.Column<Guid>(nullable: false),
            EventId = table.Column<Guid>(nullable: false),
            ParentCommand = table.Column<Guid>(nullable: true),
            CreatedAtUtc = table.Column<DateTime>(nullable: false),
            ModifiedBy = table.Column<Guid>(nullable: true),
            ModifiedAtUtc = table.Column<DateTime>(nullable: true),
            IsActive = table.Column<bool>(nullable: false)
          },

          constraints: table =>
          {
            table.PrimaryKey("PK_Comment", com => com.Id);
          });

      migrationBuilder.CreateTable(

          name: "EventImages",

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
            table.PrimaryKey("PK_EventImages", ei => ei.Id);
          });

      migrationBuilder.CreateTable(

          name: "EventFile",

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
            table.PrimaryKey("PK_Eventfile", ef => ef.Id);
          });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable("Event");
      migrationBuilder.DropTable("EventCategory");
      migrationBuilder.DropTable("Category");
      migrationBuilder.DropTable("Comment");
      migrationBuilder.DropTable("EventImages");
      migrationBuilder.DropTable("EventFile");
    }
  }
}
