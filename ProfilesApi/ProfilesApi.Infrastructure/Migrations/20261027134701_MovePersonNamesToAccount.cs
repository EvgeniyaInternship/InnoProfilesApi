using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProfilesApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MovePersonNamesToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Accounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Accounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Accounts",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Accounts" AS account
                SET "FirstName" = person."FirstName",
                    "LastName" = person."LastName",
                    "MiddleName" = person."MiddleName"
                FROM "Patients" AS person
                WHERE account."Id" = person."AccountId";
                """);

            migrationBuilder.Sql("""
                UPDATE "Accounts" AS account
                SET "FirstName" = person."FirstName",
                    "LastName" = person."LastName",
                    "MiddleName" = person."MiddleName"
                FROM "Doctors" AS person
                WHERE account."Id" = person."AccountId";
                """);

            migrationBuilder.Sql("""
                UPDATE "Accounts" AS account
                SET "FirstName" = person."FirstName",
                    "LastName" = person."LastName",
                    "MiddleName" = person."MiddleName"
                FROM "Admins" AS person
                WHERE account."Id" = person."AccountId";
                """);

            migrationBuilder.Sql("""
                UPDATE "Accounts" AS account
                SET "FirstName" = person."FirstName",
                    "LastName" = person."LastName",
                    "MiddleName" = person."MiddleName"
                FROM "Receptionists" AS person
                WHERE account."Id" = person."AccountId";
                """);

            migrationBuilder.Sql("""
                UPDATE "Accounts"
                SET "FirstName" = ''
                WHERE "FirstName" IS NULL;
                """);

            migrationBuilder.Sql("""
                UPDATE "Accounts"
                SET "LastName" = ''
                WHERE "LastName" IS NULL;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Accounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Accounts",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Receptionists");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Admins");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Admins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Receptionists",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Receptionists",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Receptionists",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Patients",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Doctors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Doctors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Doctors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Admins",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Admins",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Admins",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Patients" AS person
                SET "FirstName" = account."FirstName",
                    "LastName" = account."LastName",
                    "MiddleName" = account."MiddleName"
                FROM "Accounts" AS account
                WHERE person."AccountId" = account."Id";
                """);

            migrationBuilder.Sql("""
                UPDATE "Doctors" AS person
                SET "FirstName" = account."FirstName",
                    "LastName" = account."LastName",
                    "MiddleName" = account."MiddleName"
                FROM "Accounts" AS account
                WHERE person."AccountId" = account."Id";
                """);

            migrationBuilder.Sql("""
                UPDATE "Admins" AS person
                SET "FirstName" = account."FirstName",
                    "LastName" = account."LastName",
                    "MiddleName" = account."MiddleName"
                FROM "Accounts" AS account
                WHERE person."AccountId" = account."Id";
                """);

            migrationBuilder.Sql("""
                UPDATE "Receptionists" AS person
                SET "FirstName" = account."FirstName",
                    "LastName" = account."LastName",
                    "MiddleName" = account."MiddleName"
                FROM "Accounts" AS account
                WHERE person."AccountId" = account."Id";
                """);

            foreach (var table in new[] { "Patients", "Doctors", "Admins", "Receptionists" })
            {
                migrationBuilder.Sql($"""
                    UPDATE "{table}"
                    SET "FirstName" = ''
                    WHERE "FirstName" IS NULL;
                    """);

                migrationBuilder.Sql($"""
                    UPDATE "{table}"
                    SET "LastName" = ''
                    WHERE "LastName" IS NULL;
                    """);

                migrationBuilder.AlterColumn<string>(
                    name: "FirstName",
                    table: table,
                    type: "text",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true);

                migrationBuilder.AlterColumn<string>(
                    name: "LastName",
                    table: table,
                    type: "text",
                    nullable: false,
                    oldClrType: typeof(string),
                    oldType: "text",
                    oldNullable: true);
            }

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Accounts");
        }
    }
}
