using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky.DataAccess.Migrations
{
    public partial class addSqFtToInquiryDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SqFt",
                table: "InquiryDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SqFt",
                table: "InquiryDetail");
        }
    }
}
