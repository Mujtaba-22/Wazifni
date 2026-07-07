using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wazifni.Migrations
{
    /// <inheritdoc />
    public partial class MyProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearsExperience",
                table: "Freelancers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearsExperience",
                table: "Freelancers");
        }
    }
}
