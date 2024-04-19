using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeminarHub.Data.Migrations
{
    public partial class DeleteFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeminaarParticipants_Seminars_SeminarId",
                table: "SeminaarParticipants");

            migrationBuilder.AddForeignKey(
                name: "FK_SeminaarParticipants_Seminars_SeminarId",
                table: "SeminaarParticipants",
                column: "SeminarId",
                principalTable: "Seminars",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeminaarParticipants_Seminars_SeminarId",
                table: "SeminaarParticipants");

            migrationBuilder.AddForeignKey(
                name: "FK_SeminaarParticipants_Seminars_SeminarId",
                table: "SeminaarParticipants",
                column: "SeminarId",
                principalTable: "Seminars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
