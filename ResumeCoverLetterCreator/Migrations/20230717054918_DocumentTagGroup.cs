using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeCoverLetterCreator.Migrations
{
    /// <inheritdoc />
    public partial class DocumentTagGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagGroupId",
                table: "DocumentTags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentTags_TagGroupId",
                table: "DocumentTags",
                column: "TagGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentTags_TagGroups_TagGroupId",
                table: "DocumentTags",
                column: "TagGroupId",
                principalTable: "TagGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentTags_TagGroups_TagGroupId",
                table: "DocumentTags");

            migrationBuilder.DropIndex(
                name: "IX_DocumentTags_TagGroupId",
                table: "DocumentTags");

            migrationBuilder.DropColumn(
                name: "TagGroupId",
                table: "DocumentTags");
        }
    }
}
