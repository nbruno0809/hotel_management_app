using HotelManagement.DAL;
using HotelManagement.DAL.Entities;
using HotelManagement.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace HotelManagement.Pages
{
    public class RoomModel : PageModel
    {

        private readonly HotelContext context;

        public RoomModel(HotelContext context)
        {
            this.context = context;

		}
		private Guid TypeId;
        public RoomType Type;
        public List<RoomDto> Rooms { get; set; } = new List<RoomDto>();

        [BindProperty]
        [Required]
        [Range(1,99999)]
        public int RoomNumber { get; set; }

		public void OnGet()
        {
			TypeId = RouteData.Values["id"] != null ? Guid.Parse(RouteData.Values["id"].ToString()) : Guid.Empty;
            Load();
            
		}

        public IActionResult OnPost()
        {
            TypeId = RouteData.Values["id"] != null ? Guid.Parse(RouteData.Values["id"].ToString()) : Guid.Empty;
            if (TypeId != Guid.Empty && ModelState.IsValid)
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    bool exists = context.Rooms.Any(r => r.Number == RoomNumber.ToString());
                    if (exists)
                    {
                        ModelState.AddModelError("Room","A room with this number exists already");
                        return Page();
                    }
                    else
                    {

                        context.Rooms.Add(new Room
                        {
                            Number = RoomNumber.ToString(),
                            Active = true,
                            TypeId = TypeId
                        });
                        context.SaveChanges();


                        trans.Commit();
                    }
                }
                return RedirectToPage("/Admin/Room",new { id = TypeId });
            }
            
            return Page();
		}
        public IActionResult OnPostInactivate(Guid id)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                var room = context.Rooms.FirstOrDefault(r=>r.Id == id);
                if (room != null)
                {
                    room.Active = false;
                    context.SaveChanges();
                    trans.Commit();
                }
            }
			return RedirectToPage("/Admin/Room",TypeId);
		}

		public IActionResult OnPostActivate(Guid id)
		{
			using (var trans = context.Database.BeginTransaction())
			{
				var room = context.Rooms.FirstOrDefault(r => r.Id == id);
				if (room != null)
				{
					room.Active = true;
					context.SaveChanges();
					trans.Commit();
				}
			}
			return RedirectToPage("/Admin/Room",TypeId);
		}

		public IActionResult OnPostDelete(Guid id)
		{
			using (var trans = context.Database.BeginTransaction())
			{
				var room = context.Rooms.FirstOrDefault(r => r.Id == id);
				if (room != null)
				{
					try
                    {
                        var reservations = context.Reservations.Include(r=>r.Rooms).Where(r=>r.Rooms.Any(ro=>ro.Id == id)).ToList();                
                        reservations.ForEach(r => r.Rooms.Remove(room));

                        context.Rooms.Remove(room);

                        context.SaveChanges();
                        trans.Commit();
                    } catch (Exception ex)
                    {
                        trans.Rollback();                       
                    }
				}
			}
			return RedirectToPage("/Admin/Room",TypeId);
		}



		private void Load()
        {
                if (TypeId != Guid.Empty)
                {
                    var type = context.RoomTypes.SingleOrDefault(t => t.Id == TypeId);
                    if (type != null)
                    {
                        Type = type;
                    }

                    Rooms = context.Rooms
                       .Where(r => r.TypeId == TypeId)
                       .Include(r => r.Reservations)
                       .ThenInclude(res => res.User)
                       .Include(r => r.Type)
                       .Select(RoomDto.Mapper)
                       .ToList();
                }
            }
        
	}
}
