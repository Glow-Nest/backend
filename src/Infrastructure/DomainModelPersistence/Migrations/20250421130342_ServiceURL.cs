using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainModelPersistence.Migrations
{
    /// <inheritdoc />
    public partial class ServiceURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaUrls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaUrls_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MediaUrls_ServiceId",
                table: "MediaUrls",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaUrls");
        }
    }
}
