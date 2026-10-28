using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SentenceBuilder.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sentences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sentences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    WordTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Words_WordTypes_WordTypeId",
                        column: x => x.WordTypeId,
                        principalTable: "WordTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SentenceWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SentenceId = table.Column<int>(type: "int", nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentenceWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SentenceWords_Sentences_SentenceId",
                        column: x => x.SentenceId,
                        principalTable: "Sentences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SentenceWords_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "WordTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Noun" },
                    { 2, "Verb" },
                    { 3, "Adjective" },
                    { 4, "Adverb" },
                    { 5, "Pronoun" },
                    { 6, "Preposition" },
                    { 7, "Conjunction" },
                    { 8, "Determiner" },
                    { 9, "Exclamation" }
                });

            migrationBuilder.InsertData(
                table: "Words",
                columns: new[] { "Id", "Text", "WordTypeId" },
                values: new object[,]
                {
                    { 1, "cat", 1 },
                    { 2, "dog", 1 },
                    { 3, "teacher", 1 },
                    { 4, "house", 1 },
                    { 5, "river", 1 },
                    { 6, "book", 1 },
                    { 7, "city", 1 },
                    { 8, "music", 1 },
                    { 9, "doctor", 1 },
                    { 10, "garden", 1 },
                    { 11, "runs", 2 },
                    { 12, "eats", 2 },
                    { 13, "jumps", 2 },
                    { 14, "writes", 2 },
                    { 15, "builds", 2 },
                    { 16, "reads", 2 },
                    { 17, "sings", 2 },
                    { 18, "plays", 2 },
                    { 19, "opens", 2 },
                    { 20, "loves", 2 },
                    { 21, "happy", 3 },
                    { 22, "quick", 3 },
                    { 23, "large", 3 },
                    { 24, "quiet", 3 },
                    { 25, "bright", 3 },
                    { 26, "old", 3 },
                    { 27, "clever", 3 },
                    { 28, "brave", 3 },
                    { 29, "cold", 3 },
                    { 30, "red", 3 },
                    { 31, "quickly", 4 },
                    { 32, "slowly", 4 },
                    { 33, "happily", 4 },
                    { 34, "quietly", 4 },
                    { 35, "often", 4 },
                    { 36, "always", 4 },
                    { 37, "never", 4 },
                    { 38, "carefully", 4 },
                    { 39, "loudly", 4 },
                    { 40, "soon", 4 },
                    { 41, "I", 5 },
                    { 42, "you", 5 },
                    { 43, "he", 5 },
                    { 44, "she", 5 },
                    { 45, "it", 5 },
                    { 46, "we", 5 },
                    { 47, "they", 5 },
                    { 48, "him", 5 },
                    { 49, "her", 5 },
                    { 50, "them", 5 },
                    { 51, "in", 6 },
                    { 52, "on", 6 },
                    { 53, "under", 6 },
                    { 54, "over", 6 },
                    { 55, "with", 6 },
                    { 56, "through", 6 },
                    { 57, "between", 6 },
                    { 58, "near", 6 },
                    { 59, "behind", 6 },
                    { 60, "across", 6 },
                    { 61, "and", 7 },
                    { 62, "but", 7 },
                    { 63, "or", 7 },
                    { 64, "because", 7 },
                    { 65, "although", 7 },
                    { 66, "while", 7 },
                    { 67, "so", 7 },
                    { 68, "yet", 7 },
                    { 69, "if", 7 },
                    { 70, "since", 7 },
                    { 71, "the", 8 },
                    { 72, "a", 8 },
                    { 73, "an", 8 },
                    { 74, "this", 8 },
                    { 75, "that", 8 },
                    { 76, "these", 8 },
                    { 77, "those", 8 },
                    { 78, "my", 8 },
                    { 79, "some", 8 },
                    { 80, "every", 8 },
                    { 81, "Wow", 9 },
                    { 82, "Oops", 9 },
                    { 83, "Hooray", 9 },
                    { 84, "Ouch", 9 },
                    { 85, "Hey", 9 },
                    { 86, "Yikes", 9 },
                    { 87, "Bravo", 9 },
                    { 88, "Alas", 9 },
                    { 89, "Phew", 9 },
                    { 90, "Congratulations", 9 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SentenceWords_SentenceId_Position",
                table: "SentenceWords",
                columns: new[] { "SentenceId", "Position" });

            migrationBuilder.CreateIndex(
                name: "IX_SentenceWords_WordId",
                table: "SentenceWords",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_Words_WordTypeId_Text",
                table: "Words",
                columns: new[] { "WordTypeId", "Text" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WordTypes_Name",
                table: "WordTypes",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SentenceWords");

            migrationBuilder.DropTable(
                name: "Sentences");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "WordTypes");
        }
    }
}
