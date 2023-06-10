using HotelManagement.DAL.Entities;
using HotelManagement.DAL.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DAL
{
    public class HotelContext : IdentityDbContext<HotelUser,IdentityRole<Guid>,Guid>
    {
        public HotelContext(DbContextOptions options) : base(options) { }

        public DbSet<Extra> Extras { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Extra>(extra =>
            {
                extra.HasMany(e => e.Reservations).WithMany(r => r.Extras);
                extra.Property(e => e.Name).IsRequired().HasMaxLength(30);
                extra.Property(e => e.Description).HasMaxLength(255);
            }
            );


            builder.Entity<RoomType>().HasMany(t=>t.Rooms).WithOne(r => r.Type);

            builder.Entity<Room>().HasOne(r => r.Type).WithMany(t=>t.Rooms).HasForeignKey(r => r.TypeId);

            builder.Entity<Reservation>().HasMany(r => r.Rooms).WithMany(r => r.Reservations);
            builder.Entity<Reservation>().HasMany(r => r.Extras).WithMany(e => e.Reservations);
            builder.Entity<Reservation>().HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId);

            builder.Entity<Extra>().HasMany(e => e.Reservations).WithMany(r => r.Extras);


            builder.AddDemoData();
            
        }
    }
}
