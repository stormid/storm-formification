using Microsoft.EntityFrameworkCore.Migrations;

namespace Storm.Formification.WebWithDb.Data.Migrations
{
    public partial class formcontainer_user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FormContainer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FormContainer_UserId",
                table: "FormContainer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FormContainer_AspNetUsers_UserId",
                table: "FormContainer",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FormContainer_AspNetUsers_UserId",
                table: "FormContainer");

            migrationBuilder.DropIndex(
                name: "IX_FormContainer_UserId",
                table: "FormContainer");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FormContainer");
        }
    }
}
