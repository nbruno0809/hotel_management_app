using System.ComponentModel.DataAnnotations;

namespace HotelManagement.DTO
{
    public class ReservationPlan
    {
        public DateTime StartDate { get; set; } = DateTime.Today.AddDays(1);
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(4);
        public List<AmountOfRoomType> AmountOfRoomTypes { get; set;} = new List<AmountOfRoomType>();
        public HashSet<Guid> ExtraIds { get; set; } = new HashSet<Guid>();
    }
}
