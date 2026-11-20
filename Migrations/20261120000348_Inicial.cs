using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitizenTech.Api.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NET_CUIDADOR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Telefone = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    Tipo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_CUIDADOR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NET_IDOSO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    CuidadorId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Telefone = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    Endereco = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    CodigoDispositivo = table.Column<string>(type: "NVARCHAR2(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_IDOSO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NET_IDOSO_NET_CUIDADOR_CuidadorId",
                        column: x => x.CuidadorId,
                        principalTable: "NET_CUIDADOR",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NET_MEDICAMENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IdosoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Dosagem = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true),
                    HorarioPrevisto = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Ativo = table.Column<int>(type: "NUMBER(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_MEDICAMENTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NET_MEDICAMENTO_NET_IDOSO_IdosoId",
                        column: x => x.IdosoId,
                        principalTable: "NET_IDOSO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NET_EVENTO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    IdosoId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MedicamentoId = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Tipo = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    DataHora = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Observacao = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NET_EVENTO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NET_EVENTO_NET_IDOSO_IdosoId",
                        column: x => x.IdosoId,
                        principalTable: "NET_IDOSO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NET_EVENTO_NET_MEDICAMENTO_MedicamentoId",
                        column: x => x.MedicamentoId,
                        principalTable: "NET_MEDICAMENTO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NET_EVENTO_IdosoId",
                table: "NET_EVENTO",
                column: "IdosoId");

            migrationBuilder.CreateIndex(
                name: "IX_NET_EVENTO_MedicamentoId",
                table: "NET_EVENTO",
                column: "MedicamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_NET_IDOSO_CodigoDispositivo",
                table: "NET_IDOSO",
                column: "CodigoDispositivo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NET_IDOSO_CuidadorId",
                table: "NET_IDOSO",
                column: "CuidadorId");

            migrationBuilder.CreateIndex(
                name: "IX_NET_MEDICAMENTO_IdosoId",
                table: "NET_MEDICAMENTO",
                column: "IdosoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NET_EVENTO");

            migrationBuilder.DropTable(
                name: "NET_MEDICAMENTO");

            migrationBuilder.DropTable(
                name: "NET_IDOSO");

            migrationBuilder.DropTable(
                name: "NET_CUIDADOR");
        }
    }
}
