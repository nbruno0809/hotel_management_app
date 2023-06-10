using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DAL.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public string Number { get; set; }

        public bool Active { get; set; }

        public Guid TypeId { get; set; }
        public RoomType Type { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
