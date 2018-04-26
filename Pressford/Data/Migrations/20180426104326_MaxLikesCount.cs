using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Pressford.Data.Migrations
{
    public partial class MaxLikesCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleComment_Article_ArticleId",
                table: "ArticleComment");

            migrationBuilder.AddColumn<int>(
                name: "MaxLikesCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "ArticleComment",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleComment_Article_ArticleId",
                table: "ArticleComment",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleComment_Article_ArticleId",
                table: "ArticleComment");

            migrationBuilder.DropColumn(
                name: "MaxLikesCount",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ArticleId",
                table: "ArticleComment",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleComment_Article_ArticleId",
                table: "ArticleComment",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
