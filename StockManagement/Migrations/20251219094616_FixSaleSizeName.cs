using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockManagement.Migrations
{
    /// <inheritdoc />
    public partial class FixSaleSizeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Styles",
                newName: "StyleName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Sizes",
                newName: "SizeName");

            migrationBuilder.AddColumn<string>(
                name: "SizeName",
                table: "Stock",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SizeName",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeName",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "SizeName",
                table: "Sales");

            migrationBuilder.RenameColumn(
                name: "StyleName",
                table: "Styles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SizeName",
                table: "Sizes",
                newName: "Name");
        }
    }
}
