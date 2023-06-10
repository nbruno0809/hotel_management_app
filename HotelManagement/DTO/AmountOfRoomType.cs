namespace HotelManagement.DTO
{
    public class AmountOfRoomType
    {
        public Guid TypeId { get; set; }

        public string TypeName { get; set; } = null!;
        public int Amount { get; set; }
        
    }
}
