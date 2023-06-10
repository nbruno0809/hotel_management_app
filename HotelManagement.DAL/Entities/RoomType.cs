using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DAL.Entities
{
    public class RoomType
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double PricePerNight { get; set; }
        public int  NumberOfBeds { get; set; }
        public string? ImgPath { get; set; }

        public ICollection<Room> Rooms { get; set; }

    }
}
