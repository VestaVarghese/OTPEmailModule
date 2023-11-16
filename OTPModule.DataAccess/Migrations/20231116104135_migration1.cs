using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OTPModule.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "OTP",
                newName: "CreatedDates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDates",
                table: "OTP",
                newName: "CreatedDate");
        }
    }
}
