using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Storm.Formification.WebWithDb.Data.Migrations
{
    public partial class formcontainer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormContainer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DocumentId = table.Column<string>(nullable: true),
                    SecretId = table.Column<string>(nullable: true),
                    FormType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormContainer", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormContainer");
        }
    }
}
