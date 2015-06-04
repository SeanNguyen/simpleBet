using System.Collections.Generic;
using Microsoft.Data.Entity.Relational.Migrations;
using Microsoft.Data.Entity.Relational.Migrations.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Operations;

namespace SimpleBet.Migrations
{
    public partial class init : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateSequence(
                name: "DefaultSequence",
                type: "bigint",
                startWith: 1L,
                incrementBy: 10);
            migration.CreateTable(
                name: "User",
                columns: table => new
                {
                    FacebookId = table.Column(type: "int", nullable: true),
                    Id = table.Column(type: "int", nullable: false),
                    Name = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
            migration.CreateTable(
                name: "ValueModel",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueModel", x => x.Id);
                });
            migration.CreateTable(
                name: "Bet",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false),
                    Question = table.Column(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bet_User_UserId",
                        columns: x => x.UserId,
                        referencedTable: "User",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "Option",
                columns: table => new
                {
                    BetId = table.Column(type: "int", nullable: true),
                    Content = table.Column(type: "nvarchar(max)", nullable: true),
                    Id = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Option", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Option_Bet_BetId",
                        columns: x => x.BetId,
                        referencedTable: "Bet",
                        referencedColumn: "Id");
                });
        }
        
        public override void Down(MigrationBuilder migration)
        {
            migration.DropSequence("DefaultSequence");
            migration.DropTable("Bet");
            migration.DropTable("Option");
            migration.DropTable("User");
            migration.DropTable("ValueModel");
        }
    }
}
