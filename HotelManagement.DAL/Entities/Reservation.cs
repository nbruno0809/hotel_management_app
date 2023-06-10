using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DAL.Entities
{
    public class Reservation
    {
        public Guid  Id { get; set; }
        public DateTime? DateOfReservation { get; set; }

        public DateTime From {get; set; }
        public DateTime To { get; set; }
        //public ICollection<Guid> RoomIds { get; set; }

        public ICollection<Room> Rooms { get; set; } 

        //public ICollection<Guid> ExtraIds { get; set; }

        public ICollection<Extra> Extras { get; set; }

        public double Price { get; set; }

        public Guid UserId { get; set; }
        public HotelUser User { get; set; }

       
    }
}
