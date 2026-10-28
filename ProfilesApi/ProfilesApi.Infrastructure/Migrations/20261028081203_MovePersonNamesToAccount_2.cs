using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfilesApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MovePersonNamesToAccount_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_InsuranceNumber",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_SpecializationId",
                table: "Doctors");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceNumber",
                table: "Patients",
                column: "InsuranceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_InsuranceNumber",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceNumber",
                table: "Patients",
                column: "InsuranceNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_SpecializationId",
                table: "Doctors",
                column: "SpecializationId");
        }
    }
}
