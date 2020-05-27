using Microsoft.EntityFrameworkCore.Migrations;

namespace Microting.RebusPOC.Infrastructure.migrations.slave
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    WorkId = table.Column<string>(nullable: false),
                    Id = table.Column<long>(nullable: false),
                    WorkerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.WorkId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");
        }
    }
}
