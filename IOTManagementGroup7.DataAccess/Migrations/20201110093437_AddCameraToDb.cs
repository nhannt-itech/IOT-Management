using Microsoft.EntityFrameworkCore.Migrations;

namespace IOTManagementGroup7.DataAccess.Migrations
{
    public partial class AddCameraToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PowerStatus = table.Column<bool>(nullable: false),
                    ConnectionStatus = table.Column<bool>(nullable: false),
                    NightVersionStatus = table.Column<string>(nullable: true),
                    TimelapsRecordingStatus = table.Column<bool>(nullable: false),
                    SourceCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cameras_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_ApplicationUserId",
                table: "Cameras",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cameras");
        }
    }
}
