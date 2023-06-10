using HotelManagement.DAL;
using HotelManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace HotelManagement.Pages
{
    public class RoomsModel : PageModel
    {
        private readonly HotelContext context;

        public RoomsModel(HotelContext context)
        {
            this.context = context;
			RoomTypes =  context.RoomTypes.Include(t => t.Rooms).ToList();
		}

        public class InputModel
        {
            [Required]
            [MaxLength(50)]
            [MinLength(3)]
			public string Name { get; set; }
			[MaxLength(150)]
			
			public string Description { get; set; }
            [Required]
            [Range(0, 9999)]
			public double PricePerNight { get; set; }
            [Required]
            [Range(1,30)]
			public int NumberOfBeds { get; set; }
			public string? ImgPath { get; set; }
		}

        [BindProperty]
        public InputModel Input { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public async void OnGetAsync()
        {
            RoomTypes = await context.RoomTypes.Include(t => t.Rooms).ToListAsync();
        }

		public async Task<IActionResult> OnPostAsync()
		{

			if (Input != null && ModelState.IsValid)
            {
                RoomType roomType = new RoomType()
                {
                    Name = Input.Name,
                    Description = Input.Description,
                    NumberOfBeds = Input.NumberOfBeds,
                    PricePerNight = Input.PricePerNight,
                    ImgPath = Input.ImgPath
                    
                };
                var x = await context.RoomTypes.AddAsync(roomType);
                context.SaveChanges();

                RedirectToPage("/Rooms");
            }

            return Page();
        }
    }
}
