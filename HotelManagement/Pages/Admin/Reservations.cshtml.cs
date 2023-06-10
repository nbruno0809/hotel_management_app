using HotelManagement.DAL;
using HotelManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Pages
{
    public class ReservationsModel : PageModel
    {
        private readonly HotelContext context;

        public ReservationsModel(HotelContext context)
        {
            this.context = context;
        }

        [BindProperty]
        [MaxLength(50)]
        public string? SearchString { get; set; }
		[BindProperty]
		public DateTime? From { get; set; } = null;
        [BindProperty]
        public DateTime? To { get; set; } = null;

		public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public void OnGet()
        {
            Reservations =  context.Reservations
                //.Where(r => (string.IsNullOrEmpty(SearchString) ||
                //        r.User.FirstName.Contains(SearchString) ||
                //        r.User.LastName.Contains(SearchString)) &&
                //        (From == null || r.From>=From) &&
                //        (To == null || r.To<=To)
                //      )
                .Include(r => r.User)
                .Include(r => r.Rooms)
                .Include (r => r.Extras)
                .OrderBy(r=>r.From)
                .ToList();
        }

        public IActionResult OnPost()
        {
			Reservations = context.Reservations
			   .Where(r => (string.IsNullOrEmpty(SearchString) ||
					   r.User.FirstName.Contains(SearchString) ||
					   r.User.LastName.Contains(SearchString)) &&
					   (From == null || r.From >= From) &&
					   (To == null || r.To <= To)
					 )
			   .Include(r => r.User)
			   .Include(r => r.Rooms)
			   .Include(r => r.Extras)
			   .OrderBy(r => r.From)
			   .ToList();

            return Page();
		}
    }
}
