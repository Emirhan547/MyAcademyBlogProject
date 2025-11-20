using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blogy.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class mig_addcommentprop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsToxic",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsToxic",
                table: "Comments");
        }
    }
}
