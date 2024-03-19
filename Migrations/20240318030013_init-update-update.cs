using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be_project_swp.Migrations
{
    /// <inheritdoc />
    public partial class initupdateupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FullName_Sender",
                table: "requestorders",
                newName: "NickName_Sender");

            migrationBuilder.RenameColumn(
                name: "FullName_Receivier",
                table: "requestorders",
                newName: "NickName_Receivier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NickName_Sender",
                table: "requestorders",
                newName: "FullName_Sender");

            migrationBuilder.RenameColumn(
                name: "NickName_Receivier",
                table: "requestorders",
                newName: "FullName_Receivier");
        }
    }
}
