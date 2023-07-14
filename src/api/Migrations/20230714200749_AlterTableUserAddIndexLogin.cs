using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AlterTableUserAddIndexLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Login",
                schema: "cadastro",
                table: "Usuario",
                column: "Login");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuario_Login",
                schema: "cadastro",
                table: "Usuario");
        }
    }
}
