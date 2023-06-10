using HotelManagement.DAL;
using HotelManagement.DAL.Entities;
using HotelManagement.DTO;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HotelManagement.Services
{
    public class ReservationManagerService
    {
        private readonly HotelContext dbContext;
        private readonly IEmailSender emailSender;
        public ReservationManagerService(HotelContext hotelContext, IEmailSender emailSender) { 
            dbContext = hotelContext;
            this.emailSender = emailSender;
        }

        public async Task<List<RoomTypeAvailability>> GetAvailabilityAsync(DateTime from, DateTime to)
        {
            //Nem elérhető szobák
            var unavailableRoomsQuery = await dbContext.Reservations
                .Include(r => r.Rooms)
                    .ThenInclude(r => r.Type)
                .Where(r => r.From <= to && r.To >= from)
                .Select(r => r.Rooms)
                .ToListAsync();

            HashSet<Room> unavailableRooms = new HashSet<Room>();
            foreach (var rooms in unavailableRoomsQuery)
            {
                foreach (var room in rooms)
                {
                    unavailableRooms.Add(room);
                }
            }


            //Csoportosítás típus szerint
            var availabilityPerType = await dbContext.Rooms
                .GroupBy(r => r.TypeId)
                .Select(g => new { TypeId = g.Key,Rooms = g.ToList(), Type=g.First().Type })
                .ToListAsync();


            //Elérhető szobák = amik nincsenek a nem elérhetők közt
            List<RoomTypeAvailability> roomTypesAvailabilities = new List<RoomTypeAvailability>();
            foreach (var a in availabilityPerType)
            {
                int count = a.Rooms.Where(room => !unavailableRooms.Any(u => u.Id == room.Id) && room.Active).Count();
                //Ha van belőle elérhető bekerül
                if (count > 0)
                {
                    roomTypesAvailabilities.Add(new RoomTypeAvailability()
                    {
                        RoomType = dbContext.RoomTypes.Single(t => t.Id == a.TypeId),
                        AvailableCount = count
                    });
                }
            }

            return roomTypesAvailabilities;
        }

        public async Task<double> CalculatePrice(ReservationPlan plan)
        {           
            if (plan == null) return 0;

            double price = 0;
            foreach (var type in plan.AmountOfRoomTypes)
            {
                var roomType = await dbContext.RoomTypes.SingleOrDefaultAsync(r => r.Id == type.TypeId);
                if (roomType == null) throw new ArgumentNullException(nameof(roomType));
               price += (type.Amount * roomType.PricePerNight * ((plan.EndDate - plan.StartDate).TotalDays));
            }

            foreach (var e in plan.ExtraIds) {
                var extra = dbContext.Extras.SingleOrDefault(x => x.Id == e);
                if (extra == null)
                    throw new ArgumentNullException(nameof(extra));
                price += extra.Price * (plan.EndDate - plan.StartDate).TotalDays;
            }

            return price;
        }

        public async Task Book(HotelUser user, ReservationPlan plan)
        {
            using (var transaction = await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var extras = await dbContext.Extras.Where(x => plan.ExtraIds.Contains(x.Id)).ToListAsync();
                    List<Room> rooms = new List<Room>();

                    foreach (var aort in plan.AmountOfRoomTypes)
                    {

                        var roomsOfType = await dbContext.Rooms
                            .Where(r => 
                                r.TypeId == aort.TypeId &&
                                r.Active &&
                                !r.Reservations.Any(e => e.From <= plan.EndDate && e.To >= plan.StartDate)
                            )
                            .OrderBy(x => x.Number)
                            .Take(aort.Amount)
                            .ToListAsync();

                        if (roomsOfType == null || roomsOfType.Count() != aort.Amount)
                            throw new Exception("Not enough room");

                        rooms.AddRange(roomsOfType);


                    }

                    Reservation newReservation = new Reservation()
                    {
                        From = plan.StartDate,
                        To = plan.EndDate,
                        Extras = extras,
                        Price = await CalculatePrice(plan),
                        UserId = user.Id,
                        User = user,
                        Rooms = rooms,
                        DateOfReservation = DateTime.Now
                    };

                    dbContext.Reservations.Add(newReservation);
                    dbContext.SaveChanges();
                    transaction.Commit();
                    await SendEmail(newReservation);



                } catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private async Task SendEmail(Reservation reservation)
        {
            string htmlMessage = "Your reservation was successfull";

            await emailSender.SendEmailAsync(reservation.User.Email,"Successfull booking",htmlMessage);
        }
    }
}
