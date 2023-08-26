using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoderHive.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorPostTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Posts",
                newName: "PostTitle");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Posts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostTitle",
                table: "Posts",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
