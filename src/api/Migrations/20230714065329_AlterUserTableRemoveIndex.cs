using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class AlterUserTableRemoveIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuario_Senha",
                schema: "cadastro",
                table: "Usuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Senha",
                schema: "cadastro",
                table: "Usuario",
                column: "Senha");
        }
    }
}
