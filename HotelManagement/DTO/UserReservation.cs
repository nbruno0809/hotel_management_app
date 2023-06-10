using HotelManagement.DAL.Entities;
using System.Transactions;

namespace HotelManagement.DTO
{
    public class UserReservation
    {
        public Guid Id { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public List<string> Extras { get; set; } = null!;
        public List<AmountOfRoomType> AmountOfRoomTypes { get; set; } = null!;
        public double Price { get; set; }


        public static UserReservation MapToDto(Reservation entity)
        {
            var reservation = new UserReservation();
            reservation.Id = entity.Id;
            reservation.From = entity.From;
            reservation.To = entity.To;
            reservation.Extras = entity.Extras.Select(x=>x.Name).ToList();
            reservation.Price = entity.Price;
            reservation.AmountOfRoomTypes =
                entity.Rooms
                .GroupBy(r => r.TypeId)
                .Select(g => new AmountOfRoomType()
                {
                    TypeId = g.Key,
                    TypeName = g.First().Type.Name,
                    Amount = g.Count()
                })
                .ToList();
            return reservation;
            
        }
    }
}
