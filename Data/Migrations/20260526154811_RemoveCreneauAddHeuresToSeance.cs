using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetEMIT.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCreneauAddHeuresToSeance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seances_Creneaux_CreneauId",
                table: "Seances");

            migrationBuilder.DropIndex(
                name: "IX_Seances_Date_CreneauId_EnseignantId",
                table: "Seances");

            migrationBuilder.DropIndex(
                name: "IX_Seances_Date_CreneauId_SalleId",
                table: "Seances");

            migrationBuilder.AlterColumn<int>(
                name: "CreneauId",
                table: "Seances",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HeureDebut",
                table: "Seances",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HeureFin",
                table: "Seances",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddForeignKey(
                name: "FK_Seances_Creneaux_CreneauId",
                table: "Seances",
                column: "CreneauId",
                principalTable: "Creneaux",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Seances_Creneaux_CreneauId",
                table: "Seances");

            migrationBuilder.DropColumn(
                name: "HeureDebut",
                table: "Seances");

            migrationBuilder.DropColumn(
                name: "HeureFin",
                table: "Seances");

            migrationBuilder.AlterColumn<int>(
                name: "CreneauId",
                table: "Seances",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seances_Date_CreneauId_EnseignantId",
                table: "Seances",
                columns: new[] { "Date", "CreneauId", "EnseignantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Seances_Date_CreneauId_SalleId",
                table: "Seances",
                columns: new[] { "Date", "CreneauId", "SalleId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Seances_Creneaux_CreneauId",
                table: "Seances",
                column: "CreneauId",
                principalTable: "Creneaux",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
