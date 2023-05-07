using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class samples2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SecondSample",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RandomDeeperPropertyOne = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomDeeperPropertyTwo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomDeeperPropertyThree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomDeeperPropertyFour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomDeeperPropertyFive = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomDeeperPropertySix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomDeeperPropertySeven = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomDeeperPropertyEight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomDeeperPropertyNine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomDeeperPropertyTen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SampleEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondSample", x => x.Id)
                        .Annotation("SqlServer:Clustered", false);
                    table.ForeignKey(
                        name: "FK_SecondSample_Samples_SampleEntityId",
                        column: x => x.SampleEntityId,
                        principalTable: "Samples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SecondSample_SampleEntityId",
                table: "SecondSample",
                column: "SampleEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SecondSample");
        }
    }
}
