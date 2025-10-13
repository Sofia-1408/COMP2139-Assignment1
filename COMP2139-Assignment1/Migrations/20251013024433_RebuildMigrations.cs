using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace COMP2139_Assignment1.Migrations
{
    /// <inheritdoc />
    public partial class RebuildMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TicketPrice = table.Column<double>(type: "double precision", nullable: false),
                    AvailableTickets = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_Events_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalCost = table.Column<double>(type: "double precision", nullable: false),
                    GuestContactInfo = table.Column<string>(type: "text", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchases_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Concerts and live music events.", "Music" },
                    { 2, "Sports games and tournaments.", "Sports" },
                    { 3, "Plays and live performances.", "Theatre" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "EventId", "AvailableTickets", "CategoryId", "Date", "TicketPrice", "Title" },
                values: new object[,]
                {
                    { 1, 250, 1, new DateTime(2025, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), 75.0, "Summer Beats Festival" },
                    { 2, 500, 2, new DateTime(2025, 9, 10, 0, 0, 0, 0, DateTimeKind.Utc), 30.0, "City Marathon 2025" },
                    { 3, 150, 3, new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), 40.0, "Shakespeare in the Park" }
                });

            migrationBuilder.InsertData(
                table: "Purchases",
                columns: new[] { "PurchaseId", "EventId", "GuestContactInfo", "PurchaseDate", "TotalCost" },
                values: new object[,]
                {
                    { 1, 1, "Name: Alice Johnson, Email: alice@example.com", new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 150.0 },
                    { 2, 2, "Name: Bob Smith, Email: bob.example.com", new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Utc), 60.0 },
                    { 3, 3, "Name: Carol Lee, Email: carol.example.com", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), 80.0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CategoryId",
                table: "Events",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_EventId",
                table: "Purchases",
                column: "EventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
