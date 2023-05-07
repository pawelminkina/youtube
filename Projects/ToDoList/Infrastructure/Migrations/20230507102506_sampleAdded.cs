using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sampleAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Samples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RandomPropertyOne = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomPropertyTwo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomPropertyThree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomPropertyFour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomPropertyFive = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomPropertySix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomPropertySeven = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomPropertyEight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomPropertyNine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomPropertyTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ToDoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Samples", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_Samples_ToDoItems_ToDoId",
                        column: x => x.ToDoId,
                        principalTable: "ToDoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Samples_ToDoId",
                table: "Samples",
                column: "ToDoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Samples");
        }
    }
}
