using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tria_2025.Migrations
{
    /// <inheritdoc />
    public partial class AddReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Text = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    PredictedSentiment = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SentimentScore = table.Column<float>(type: "BINARY_FLOAT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reviews");
        }
    }
}
