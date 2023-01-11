using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TraderProxy.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketStateSnaps",
                columns: table => new
                {
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Trade = table.Column<int>(type: "int", nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstrumentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BestBid = table.Column<double>(type: "float", nullable: true),
                    BestAsk = table.Column<double>(type: "float", nullable: true),
                    Actual = table.Column<double>(type: "float", nullable: true),
                    Low = table.Column<double>(type: "float", nullable: true),
                    High = table.Column<double>(type: "float", nullable: true),
                    Volume = table.Column<double>(type: "float", nullable: false),
                    Change = table.Column<double>(type: "float", nullable: true),
                    Timestamp = table.Column<long>(type: "bigint", nullable: false),
                    BigVolume = table.Column<double>(type: "float", nullable: false),
                    PartChange = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketStateSnaps", x => x.DateTime);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketStateSnaps");
        }
    }
}
