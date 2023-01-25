using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rocky.DataAccess.Migrations
{
    public partial class updateInquiryDetailSqFt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "SqFt",
                table: "InquiryDetail",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SqFt",
                table: "InquiryDetail",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
