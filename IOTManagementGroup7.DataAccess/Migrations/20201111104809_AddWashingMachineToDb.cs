using Microsoft.EntityFrameworkCore.Migrations;

namespace IOTManagementGroup7.DataAccess.Migrations
{
    public partial class AddWashingMachineToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WashingMachine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    PowerStatus = table.Column<bool>(nullable: false),
                    ProgramStatus = table.Column<string>(nullable: true),
                    Weight = table.Column<double>(nullable: false),
                    RemainingTime = table.Column<int>(nullable: false),
                    SourceCode = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WashingMachine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WashingMachine_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WashingMachine_ApplicationUserId",
                table: "WashingMachine",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WashingMachine");
        }
    }
}
