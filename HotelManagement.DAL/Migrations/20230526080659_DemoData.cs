using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManagement.DAL.Migrations
{
    public partial class DemoData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Rooms");

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("1c03a7ec-5fd4-46d7-a189-9fcf9715dd40"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("789c69bf-6430-465a-bfa0-96fe0eaf134c"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("cf2b8438-a9bb-4975-84dc-c64bcfcfce42"));

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "Rooms");

            migrationBuilder.InsertData(
                table: "Extras",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("0af4261d-8ef7-49b6-8955-8c58b4a3c876"), "Refreshing full body thai massage for 90 minutes", "Massage", 8.0 },
                    { new Guid("6f21797b-8247-44d4-b194-48a9c50a7a31"), "Daily pass for all the wellness services and thermal pools", "Wellness", 8.0 },
                    { new Guid("a5def5da-b0fa-457a-8378-3c330d27be3a"), "Continental breakfast with baked goods, fruit, coffee and other beverages and much more", "Breakfast", 8.0 },
                    { new Guid("efd65810-a773-4e6c-9eb0-6ddbb9fb0769"), "Daily pass for the hotels exclusive and well-equipped gym", "Gym", 8.0 }
                });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Description", "ImgPath", "Name", "NumberOfBeds", "PricePerNight" },
                values: new object[,]
                {
                    { new Guid("8ae84920-570c-47d3-b44d-959bf64c546b"), "Family sized room for four", "https://images.trvl-media.com/lodging/39000000/38830000/38822300/38822281/c7875785.jpg?impolicy=fcrop&w=1200&h=800&p=1&q=medium", "Deluxe room", 4, 30.0 },
                    { new Guid("a7073dae-4714-46f6-bac1-648e2a59c04f"), "Double room for two", "https://static01.nyt.com/images/2019/03/24/travel/24trending-shophotels1/24trending-shophotels1-superJumbo.jpg", "Standard room", 2, 20.0 },
                    { new Guid("abd84d22-32a6-4518-8108-5e82e0d7c985"), "Single room for one", "https://www.hotelmonterey.co.jp/upload_file/monhtyo/stay/sng_600_001.jpg", "Economy room", 1, 15.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Extras",
                keyColumn: "Id",
                keyValue: new Guid("0af4261d-8ef7-49b6-8955-8c58b4a3c876"));

            migrationBuilder.DeleteData(
                table: "Extras",
                keyColumn: "Id",
                keyValue: new Guid("6f21797b-8247-44d4-b194-48a9c50a7a31"));

            migrationBuilder.DeleteData(
                table: "Extras",
                keyColumn: "Id",
                keyValue: new Guid("a5def5da-b0fa-457a-8378-3c330d27be3a"));

            migrationBuilder.DeleteData(
                table: "Extras",
                keyColumn: "Id",
                keyValue: new Guid("efd65810-a773-4e6c-9eb0-6ddbb9fb0769"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("8ae84920-570c-47d3-b44d-959bf64c546b"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("a7073dae-4714-46f6-bac1-648e2a59c04f"));

            migrationBuilder.DeleteData(
                table: "RoomTypes",
                keyColumn: "Id",
                keyValue: new Guid("abd84d22-32a6-4518-8108-5e82e0d7c985"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomTypeId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Description", "ImgPath", "Name", "NumberOfBeds", "PricePerNight" },
                values: new object[] { new Guid("1c03a7ec-5fd4-46d7-a189-9fcf9715dd40"), "Room for four. One double bed and two single bed", null, "Family room", 4, 20.0 });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Description", "ImgPath", "Name", "NumberOfBeds", "PricePerNight" },
                values: new object[] { new Guid("789c69bf-6430-465a-bfa0-96fe0eaf134c"), "Room for two, with large spaces ", null, "Large double bed room", 2, 15.0 });

            migrationBuilder.InsertData(
                table: "RoomTypes",
                columns: new[] { "Id", "Description", "ImgPath", "Name", "NumberOfBeds", "PricePerNight" },
                values: new object[] { new Guid("cf2b8438-a9bb-4975-84dc-c64bcfcfce42"), "Basic room for two", null, "Small double bed room", 2, 10.0 });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomTypes_RoomTypeId",
                table: "Rooms",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "Id");
        }
    }
}
