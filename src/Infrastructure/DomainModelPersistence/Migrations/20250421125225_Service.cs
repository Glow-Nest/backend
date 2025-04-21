using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainModelPersistence.Migrations
{
    /// <inheritdoc />
    public partial class Service : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Service",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Duration",
                table: "Service",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Service",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Service",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Service");
        }
    }
}
