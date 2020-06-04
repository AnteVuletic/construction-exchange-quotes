using Microsoft.EntityFrameworkCore.Migrations;

namespace ConstructionExchangeQuotes.Server.Migrations
{
    public partial class quote_addarchivedflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Quotes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Quotes");
        }
    }
}
