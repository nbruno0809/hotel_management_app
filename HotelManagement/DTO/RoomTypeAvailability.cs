using HotelManagement.DAL.Entities;

namespace HotelManagement.DTO
{
    public class RoomTypeAvailability
    {
        public RoomType RoomType { get; set; } = null!;
        public int AvailableCount { get; set; }
    }
}
