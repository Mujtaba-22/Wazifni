using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazifni.Migrations
{
    /// <inheritdoc />
    public partial class SyncCurrentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Freelancers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_SkillId",
                table: "Freelancers",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Freelancers_UserId",
                table: "Freelancers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Freelancers_AspNetUsers_UserId",
                table: "Freelancers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Freelancers_Skills_SkillId",
                table: "Freelancers",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Freelancers_AspNetUsers_UserId",
                table: "Freelancers");

            migrationBuilder.DropForeignKey(
                name: "FK_Freelancers_Skills_SkillId",
                table: "Freelancers");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_SkillId",
                table: "Freelancers");

            migrationBuilder.DropIndex(
                name: "IX_Freelancers_UserId",
                table: "Freelancers");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Freelancers");
        }
    }
}
