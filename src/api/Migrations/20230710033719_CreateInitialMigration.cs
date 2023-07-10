using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class CreateInitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cadastro");

            migrationBuilder.CreateTable(
                name: "Projeto",
                schema: "cadastro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    Descricao = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projeto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "cadastro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Login = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tempo",
                schema: "cadastro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tempo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tempo_Projeto_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "cadastro",
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tempo_Usuario_UserId",
                        column: x => x.UserId,
                        principalSchema: "cadastro",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioProjeto",
                schema: "cadastro",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioProjeto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioProjeto_Projeto_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "cadastro",
                        principalTable: "Projeto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuarioProjeto_Usuario_UserId",
                        column: x => x.UserId,
                        principalSchema: "cadastro",
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tempo_ProjectId",
                schema: "cadastro",
                table: "Tempo",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Tempo_UserId",
                schema: "cadastro",
                table: "Tempo",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                schema: "cadastro",
                table: "Usuario",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Senha",
                schema: "cadastro",
                table: "Usuario",
                column: "Senha");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioProjeto_ProjectId",
                schema: "cadastro",
                table: "UsuarioProjeto",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioProjeto_UserId",
                schema: "cadastro",
                table: "UsuarioProjeto",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tempo",
                schema: "cadastro");

            migrationBuilder.DropTable(
                name: "UsuarioProjeto",
                schema: "cadastro");

            migrationBuilder.DropTable(
                name: "Projeto",
                schema: "cadastro");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "cadastro");
        }
    }
}
