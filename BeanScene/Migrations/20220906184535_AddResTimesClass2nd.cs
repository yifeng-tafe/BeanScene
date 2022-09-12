using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeanScene.Migrations
{
    public partial class AddResTimesClass2nd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservationTime",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationTime_ReservationType_ReservationTypeId",
                        column: x => x.ReservationTypeId,
                        principalTable: "ReservationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationTime_ReservationTypeId",
                table: "ReservationTime",
                column: "ReservationTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationTime");
        }
    }
}
