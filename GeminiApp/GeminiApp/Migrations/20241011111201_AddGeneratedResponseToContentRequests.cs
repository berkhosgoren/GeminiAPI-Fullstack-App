using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeminiApp.Migrations
{
    /// <inheritdoc />
    public partial class AddGeneratedResponseToContentRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneratedResponse",
                table: "ContentRequests",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratedResponse",
                table: "ContentRequests");
        }
    }
}
