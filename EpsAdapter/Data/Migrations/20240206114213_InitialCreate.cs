using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EpsAdapter.Data.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "eps");

        migrationBuilder.CreateTable(
            name: "CardRequests",
            schema: "eps",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Status = table.Column<int>(type: "int", nullable: false),
                CreatedOnUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                Operation = table.Column<int>(type: "int", nullable: false),
                SmartCheckCardId = table.Column<int>(type: "int", nullable: false),
                ParametersPayload = table.Column<string>(type: "nvarchar(max)", nullable: false),
                EpsCardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CardRequests", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Cards",
            schema: "eps",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SmartCheckCardId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cards", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CardRequests",
            schema: "eps");

        migrationBuilder.DropTable(
            name: "Cards",
            schema: "eps");
    }
}
