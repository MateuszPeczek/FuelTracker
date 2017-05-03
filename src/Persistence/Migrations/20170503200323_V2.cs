using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumptionReport_AspNetUsers_User",
                table: "ConsumptionReport");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsumptionReport_Vehicle_Vehicle",
                table: "ConsumptionReport");

            migrationBuilder.RenameColumn(
                name: "Vehicle",
                table: "ConsumptionReport",
                newName: "VehicleId");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "ConsumptionReport",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumptionReport_Vehicle",
                table: "ConsumptionReport",
                newName: "IX_ConsumptionReport_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumptionReport_User",
                table: "ConsumptionReport",
                newName: "IX_ConsumptionReport_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumptionReport_AspNetUsers_UserId",
                table: "ConsumptionReport",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumptionReport_Vehicle_VehicleId",
                table: "ConsumptionReport",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumptionReport_AspNetUsers_UserId",
                table: "ConsumptionReport");

            migrationBuilder.DropForeignKey(
                name: "FK_ConsumptionReport_Vehicle_VehicleId",
                table: "ConsumptionReport");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "ConsumptionReport",
                newName: "Vehicle");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ConsumptionReport",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumptionReport_VehicleId",
                table: "ConsumptionReport",
                newName: "IX_ConsumptionReport_Vehicle");

            migrationBuilder.RenameIndex(
                name: "IX_ConsumptionReport_UserId",
                table: "ConsumptionReport",
                newName: "IX_ConsumptionReport_User");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumptionReport_AspNetUsers_User",
                table: "ConsumptionReport",
                column: "User",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumptionReport_Vehicle_Vehicle",
                table: "ConsumptionReport",
                column: "Vehicle",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
