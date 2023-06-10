using HotelManagement.DAL.Entities;

namespace HotelManagement.DTO
{
	public class RoomDto
	{
		public Guid Id { get; set; }
		public string Number { get; set; }

		public bool Active { get; set; }

		public string TypeName { get; set; } = null!;
		
		public bool CurrentlyFree { get; set; }

		public string? CurrentGuest { get; set; }

		public int FutureReservationCount { get; set; }
		

		public static RoomDto Mapper(Room room)
		{
			var reservation = room.Reservations.FirstOrDefault(r => r.From <= DateTime.Now && r.To >= DateTime.Now);

			RoomDto dto = new RoomDto()
			{
				Id = room.Id,
				Number = room.Number,
				Active = room.Active,
				TypeName = room.Type.Name,
				CurrentlyFree = reservation == null,
				CurrentGuest = reservation == null ? null : reservation.User.FirstName + " " + reservation.User.LastName,
				FutureReservationCount = room.Reservations.Where(r=>r.From>DateTime.Now).Count()
			};

			return dto;
		}
	}
}
