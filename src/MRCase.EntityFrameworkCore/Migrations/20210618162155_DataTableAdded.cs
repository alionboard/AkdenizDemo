using Microsoft.EntityFrameworkCore.Migrations;

namespace MRCase.EntityFrameworkCore.Migrations
{
    public partial class DataTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Data",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Event = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Data_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2301D884-221A-4E7D-B509-0113DCC043E1",
                column: "ConcurrencyStamp",
                value: "34f0e910-72ea-4272-b091-bf533846de81");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8fa3f54e-a883-4fd5-a987-c00f90caea39", "AQAAAAEAACcQAAAAEFWlw0B3eZVTaeEieisylE6rbLBvq1sw+rAL7HHt5N6U5/CuwE3Z2O/C9Kp0ASqP2w==" });

            migrationBuilder.CreateIndex(
                name: "IX_Data_UserId",
                table: "Data",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Data");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2301D884-221A-4E7D-B509-0113DCC043E1",
                column: "ConcurrencyStamp",
                value: "4f4ae3db-1769-4247-a6cb-baac98ac3cc2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "B22698B8-42A2-4115-9631-1C2D1E2AC5F7",
                columns: new[] { "ConcurrencyStamp", "FullName", "PasswordHash" },
                values: new object[] { "3d7ed0cb-91e2-47d4-81e7-31b131095935", "admin", "AQAAAAEAACcQAAAAEO+hqdpOOSrsm6ABkLt2wPQA65cUA5zwyhbKSsLKgHw338R9MuJ/vzQxHJEvDiPA5A==" });
        }
    }
}
