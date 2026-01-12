using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_writer_appuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "Writers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Writers_AppUserId",
                table: "Writers",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Writers_AspNetUsers_AppUserId",
                table: "Writers",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Writers_AspNetUsers_AppUserId",
                table: "Writers");

            migrationBuilder.DropIndex(
                name: "IX_Writers_AppUserId",
                table: "Writers");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Writers");
        }
    }
}
