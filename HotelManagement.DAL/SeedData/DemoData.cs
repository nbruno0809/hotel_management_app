using HotelManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace HotelManagement.DAL.SeedData
{
	public static class DemoData
	{
		public static ModelBuilder AddDemoData(this ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<RoomType>().HasData(new[] {
				new RoomType() {
					Id = Guid.NewGuid(),
					Name = "Economy room",
					Description = "Single room for one",
					NumberOfBeds = 1,
					PricePerNight = 15,
					ImgPath = "https://www.hotelmonterey.co.jp/upload_file/monhtyo/stay/sng_600_001.jpg"
				},

				new RoomType() {
					Id = Guid.NewGuid(),
					Name = "Standard room",
					Description = "Double room for two",
					NumberOfBeds = 2,
					PricePerNight = 20,
					ImgPath = "https://static01.nyt.com/images/2019/03/24/travel/24trending-shophotels1/24trending-shophotels1-superJumbo.jpg"
				},

				new RoomType() {
					Id = Guid.NewGuid(),
					Name = "Deluxe room",
					Description = "Family sized room for four",
					NumberOfBeds = 4,
					PricePerNight = 30,
					ImgPath = "https://images.trvl-media.com/lodging/39000000/38830000/38822300/38822281/c7875785.jpg?impolicy=fcrop&w=1200&h=800&p=1&q=medium"
				} 
			});

			//// EXTRAS
			modelBuilder.Entity<Extra>().HasData(new[] {
				new Extra() {
					Id = Guid.NewGuid(),
					Name = "Breakfast",
					Description = "Continental breakfast with baked goods, fruit, coffee and other beverages and much more",
					Price = 8,					
				},

				new Extra() {
					Id = Guid.NewGuid(),
					Name = "Massage",
					Description = "Refreshing full body thai massage for 90 minutes",
					Price = 8,
				},

				new Extra() {
					Id = Guid.NewGuid(),
					Name = "Gym",
					Description = "Daily pass for the hotels exclusive and well-equipped gym",
					Price = 8,
				},

				new Extra() {
					Id = Guid.NewGuid(),
					Name = "Wellness",
					Description = "Daily pass for all the wellness services and thermal pools",
					Price = 8,
				},
			});


			return modelBuilder;
		}
	}
}
