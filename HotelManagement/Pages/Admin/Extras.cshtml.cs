using HotelManagement.DAL;
using HotelManagement.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Pages
{
    public class ExtrasModel : PageModel
    {
        private readonly HotelContext context;
        private readonly ILogger<ExtrasModel> logger;

        public ExtrasModel(HotelContext context, ILogger<ExtrasModel> logger) { this.context = context; this.logger = logger; }
        public List<Extra> Extras { get; set; }

        [BindProperty]
        public Input NewExtra { get; set; }
        public class Input
        {
            [Required]
            [StringLength(20)]
            public string Name { get; set; }
            [StringLength(100)]
            public string Description { get; set; }
            [Required]
            [Range(0,9999)]
            public double Price { get; set; }
        }

        public void OnGet()
        {
            Extras = context.Extras.Include(e=>e.Reservations).ToList();
        }


        public IActionResult OnPost()
        {
           if (ModelState.IsValid)
              try 
                {
                    context.Add(new Extra
                    {
                        Name = NewExtra.Name,
                        Description = NewExtra.Description,
                        Price = NewExtra.Price,                       
                    });
                    context.SaveChanges();
					return RedirectToPage("Extras");
				} catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                }
            return Page();
        }

        public IActionResult OnPostDelete(Guid id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                var extra = context.Extras.Include(e=>e.Reservations).SingleOrDefault(e=>e.Id == id);
                if (extra != null && extra.Reservations.Count() == 0)
                {
                    context.Extras.Remove(extra);
                    context.SaveChanges();
                    transaction.Commit();
                }
                return RedirectToPage("/Admin/Extras");
            }
        }

    }
}
