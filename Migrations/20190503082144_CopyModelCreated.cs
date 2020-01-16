using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NancyApplication.Migrations
{
    public partial class CopyModelCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookID",
                table: "Loans",
                newName: "CopyID");

            migrationBuilder.CreateTable(
                name: "Copy",
                columns: table => new
                {
                    CopyID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Copy", x => x.CopyID);
                    table.ForeignKey(
                        name: "FK_Copy_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loans_CopyID",
                table: "Loans",
                column: "CopyID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Copy_BookID",
                table: "Copy",
                column: "BookID");

            migrationBuilder.Sql(
                @"
                    INSERT INTO Copy (CopyID, BookID)
                    SELECT BookID AS CopyID, BookID
                    FROM Books;
                ");


            
            
            migrationBuilder.DropIndex(
                name: "IX_Loans_BookID",
                table: "Loans");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Copy_CopyID",
                table: "Loans");

            migrationBuilder.DropTable(
                name: "Copy");

            migrationBuilder.DropIndex(
                name: "IX_Loans_CopyID",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "CopyID",
                table: "Loans",
                newName: "BookID");

            migrationBuilder.AddColumn<DateTime>(
                name: "LoanDate",
                table: "Members",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Loans_BookID",
                table: "Loans",
                column: "BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Books_BookID",
                table: "Loans",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
