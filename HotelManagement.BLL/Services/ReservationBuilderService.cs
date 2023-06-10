using HotelManagement.DAL;
using HotelManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.BLL.Services
{
    public class ReservationBuilderService
    {
        private readonly HotelContext context;

        public List<RoomTypesAvailability> roomTypesAvailabilities= new List<RoomTypesAvailability>();
        private List<Extra> extras = new List<Extra>();

        private DateTime? startDate;
        private DateTime? endDate;
        
        
        public ReservationBuilderService(HotelContext context) { 
            this.context = context;
            UpdateAvailabilityAsync().Wait();
        }

        public void SetStartDate(DateTime date)
        {
            startDate = date;
        }

        public void SetEndDate(DateTime date)
        {
            endDate = date;
        }

        public DateTime? GetDate()
        {
            return startDate;
        }

        public double GetPrice()
        {
            if (startDate == null || endDate == null)
            {
                return 0;
            }

            double price = 0;
            foreach (var type  in roomTypesAvailabilities)
            {
                if (type.ReservedCount > 0)
                {
                    price += (type.ReservedCount * type.RoomType.PricePerNight * ((endDate-startDate).Value.TotalDays));
                }
            }

            extras.ForEach(e=> price += e.Price);     
            
            return price;
        }

        public async void AddExtraAsync(Guid id)
        {
            var extra = await context.Extras.SingleOrDefaultAsync(e => e.Id == id);
            if (extra != null)
            {
                extras.Add(extra);
            }
        }

        public void AddRoomOfType(Guid typeId, int amount) {
            roomTypesAvailabilities.ForEach(x =>
            {
                if (x.RoomType.Id == typeId)
                {
                    if (x.ReservedCount+amount >= x.AvailableCount )
                    {
                        x.ReservedCount = x.AvailableCount;
                        return;
                    } else if (x.ReservedCount+amount<= 0)
                    {
                        x.ReservedCount = 0;
                        return;
                        
                    } else
                    {
                        x.ReservedCount+=amount;
                        return;
                    }
                }
            });
        }

        public async Task UpdateAvailabilityAsync()
        {
            if (startDate == null || endDate == null)
                return;

            var unavailableRoomsQuery = await context.Reservations
                .Include(r => r.Rooms)
                    .ThenInclude(r => r.Type)
                .Where(r => r.From <= endDate && r.To >= startDate)
                .Select(r => r.Rooms)
                .ToListAsync();

            HashSet<Room> unavailableRooms  = new HashSet<Room>();
            foreach (var rooms in unavailableRoomsQuery)
            {
                foreach (var room in rooms)
                {
                    unavailableRooms.Add(room);
                }
            }

            

            var availabilityPerType = await context.Rooms               
                .GroupBy(r => r.TypeId)
                .Select(g => new { TypeId = g.Key,Rooms = g.ToList() })
                .ToListAsync();

            

            roomTypesAvailabilities.Clear();
            foreach (var a in availabilityPerType)
            {
                var x = a.Rooms.Where(room => !unavailableRooms.Any(u => u.Id == room.Id));


                roomTypesAvailabilities.Add(new RoomTypesAvailability()
                    {
                        RoomType = context.RoomTypes.Single(t=>t.Id==a.TypeId),
                        AvailableCount = a.Rooms.Where(room=>!unavailableRooms.Any(u=>u.Id == room.Id)).Count(),
                        ReservedCount = 0
                    }
                );
            }

        }

        public async Task<bool> BookAsync(HotelUser user)
        {
            List<Room> roomList = new List<Room>();
            using (var transaction = context.Database.BeginTransaction())
            {

                try
                {
                    foreach (var i in roomTypesAvailabilities)
                    {
                        roomList.Concat(
                            await context.Rooms
                            .Where(r => r.TypeId == i.RoomType.Id)
                            .Take(i.ReservedCount)
                            .ToListAsync()
                        );
                    }

                    Reservation reservation = new Reservation()
                    {
                        From = startDate.Value,
                        To = endDate.Value,
                        Price = GetPrice(),
                        Extras = extras,
                        Rooms = roomList,
                        User = user,
                        UserId = user.Id,
                        DateOfReservation = DateTime.Now

                    };

                    await context.Reservations.AddAsync(reservation);
                    context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch 
                {
                    transaction.Rollback();
                    return false;
                }
            }
            

        }

        public List<RoomTypesAvailability> GetRoomTypesAvailabilities()
        {
            return roomTypesAvailabilities;
        }





        public class RoomTypesAvailability
        {
            public RoomType RoomType { get; set; }
            public int AvailableCount { get; set; }
            public int ReservedCount { get; set; }
        }
    }
}
