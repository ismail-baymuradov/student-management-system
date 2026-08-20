using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddSemester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    RegistrationStart = table.Column<DateOnly>(type: "date", nullable: false),
                    RegistrationEnd = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                    table.CheckConstraint("CK_Semesters_RegistrationEndsWithinSemester", "[RegistrationEnd] <= [EndDate]");
                    table.CheckConstraint("CK_Semesters_RegistrationStartBeforeEnd", "[RegistrationStart] < [RegistrationEnd]");
                    table.CheckConstraint("CK_Semesters_RegistrationStartsBeforeSemester", "[RegistrationStart] <= [StartDate]");
                    table.CheckConstraint("CK_Semesters_StartBeforeEnd", "[StartDate] < [EndDate]");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Semesters");
        }
    }
}
