using Microsoft.EntityFrameworkCore.Migrations;

namespace IOTManagementGroup7.DataAccess.Migrations
{
    public partial class FixAddAppliancesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "TVs",
                 columns: table => new
                 {
                     Id = table.Column<int>(nullable: false)
                         .Annotation("SqlServer:Identity", "1, 1"),
                     Name = table.Column<string>(nullable: true),
                     Status = table.Column<bool>(nullable: false),
                     Channel = table.Column<string>(nullable: true),
                     Volume = table.Column<int>(nullable: true),

                 },
                  constraints: table =>
                  {
                      table.PrimaryKey("PK_TVs", x => x.Id);
                  });
            migrationBuilder.CreateTable(
                name: "Fridges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Temperature = table.Column<double>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fridges", x => x.Id);
                });
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
             name: "TVs");
            migrationBuilder.DropTable(
               name: "Fridges");
        }
    }
}
