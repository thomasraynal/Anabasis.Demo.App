using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Anabasis.Demo.Actor.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AggregateSnapshots",
                columns: table => new
                {
                    StreamId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventFilter = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    LastModifiedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SerializedAggregate = table.Column<string>(type: "nvarchar(max)", maxLength: 2147483647, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggregateSnapshots", x => new { x.StreamId, x.EventFilter, x.Version });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AggregateSnapshots");
        }
    }
}
