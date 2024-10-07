﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoreMath.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuthorUrlChangedToSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "SlugName",
                table: "Authors",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SlugName",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Authors",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
