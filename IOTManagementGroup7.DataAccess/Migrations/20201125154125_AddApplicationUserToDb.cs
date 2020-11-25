using Microsoft.EntityFrameworkCore.Migrations;

namespace IOTManagementGroup7.DataAccess.Migrations
{
    public partial class AddApplicationUserToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirConditioners");

            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropTable(
                name: "Fans");

            migrationBuilder.DropTable(
                name: "Fridge");

            migrationBuilder.DropTable(
                name: "Lights");

            migrationBuilder.DropTable(
                name: "Television");

            migrationBuilder.DropTable(
                name: "WashingMachine");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirConditioners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerStatus = table.Column<bool>(type: "bit", nullable: false),
                    SourceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temperature = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirConditioners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirConditioners_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConnectionStatus = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NightVersionStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerStatus = table.Column<bool>(type: "bit", nullable: false),
                    SourceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimelapsRecordingStatus = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cameras_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConnectionStatus = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerStatus = table.Column<bool>(type: "bit", nullable: false),
                    SourceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fans_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fridge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temperature = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fridge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fridge_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConnectionStatus = table.Column<bool>(type: "bit", nullable: false),
                    Dim = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PowerStatus = table.Column<bool>(type: "bit", nullable: false),
                    SourceCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoltageRange = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lights_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Television",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentChannel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerStatus = table.Column<bool>(type: "bit", nullable: false),
                    RecordingStatus = table.Column<bool>(type: "bit", nullable: false),
                    SourceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Volume = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Television", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Television_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WashingMachine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PowerStatus = table.Column<bool>(type: "bit", nullable: false),
                    ProgramStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemainingTime = table.Column<int>(type: "int", nullable: false),
                    SourceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: false)
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
                name: "IX_AirConditioners_ApplicationUserId",
                table: "AirConditioners",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_ApplicationUserId",
                table: "Cameras",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Fans_ApplicationUserId",
                table: "Fans",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Fridge_ApplicationUserId",
                table: "Fridge",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lights_ApplicationUserId",
                table: "Lights",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Television_ApplicationUserId",
                table: "Television",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WashingMachine_ApplicationUserId",
                table: "WashingMachine",
                column: "ApplicationUserId");
        }
    }
}
