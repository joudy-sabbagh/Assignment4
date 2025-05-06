using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyMVCApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEventPricing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BackstagePrice",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NormalPrice",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VIPPrice",
                table: "Events",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackstagePrice",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "NormalPrice",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "VIPPrice",
                table: "Events");
        }
    }
}
