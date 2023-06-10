using FluentValidation;
using HotelManagement.DAL;
using HotelManagement.DAL.Entities;
using HotelManagement.DTO;
using HotelManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System;
using System.Text.Json;

namespace HotelManagement.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HotelContext context;
        private readonly IValidator<ReservationPlan> reservationPlanValidator;
        private readonly CurrencyConverterService currencyConverter;
        public readonly ReservationManagerService reservationManager;
        public readonly UserManager<HotelUser> userManager;
        public readonly SignInManager<HotelUser> signInManager;

        public List<RoomTypeAvailability> roomTypesAvailabilities {  get; private set; } = new List<RoomTypeAvailability>();

        [BindProperty] 
        public ReservationPlan ReservationPlan { get; set; }

        [BindProperty]
        public string? TargetCurrencyCode { get; set; } = null;

        [BindProperty]
        public List<Guid> SelectedExtras { get; set; }

        public double? CurrValue { get; set; } = null;

        public IndexModel(
            ILogger<IndexModel> logger, 
            HotelContext context, 
            IValidator<ReservationPlan> validator,
            UserManager<HotelUser> userManager,
            SignInManager<HotelUser> signInManager,
            IConfiguration configuration,
			ReservationManagerService reservationManager,
            CurrencyConverterService currencyConverter
			)
        {
            _logger = logger;
            reservationPlanValidator = validator;
            this.context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.reservationManager = reservationManager;
            this.currencyConverter = currencyConverter;
           
        }
        
        /////////////////////////////////////////////////////////////////////
        
        public async void OnGet()
        {
            RestorePlanFromCookie();
        }

        public async Task<IActionResult> OnPostDateSelect()
        {
            var validationResult = await reservationPlanValidator.ValidateAsync(ReservationPlan);

            if (!validationResult.IsValid)
            {
                ModelState.AddModelError(validationResult.Errors.First().ErrorCode,validationResult.Errors.First().ErrorMessage);   
            }

            if (!ModelState.IsValid )
            {
                return Page();
            }

            ReservationPlan.AmountOfRoomTypes = new List<AmountOfRoomType>();
            SavePlanInCookie();
            roomTypesAvailabilities = await reservationManager.GetAvailabilityAsync(ReservationPlan.StartDate,ReservationPlan.EndDate);

            return Page();
        }

        public async Task<IActionResult> OnPostAddRoom(Guid typeId)
        {

            if (!RestorePlanFromCookie())
                return Page();

            var typeAmount = ReservationPlan.AmountOfRoomTypes.SingleOrDefault(x => x.TypeId == typeId);
            if (typeAmount != null)
            {
                typeAmount.Amount++;
                
            } else
            {
                var roomType = context.RoomTypes.SingleOrDefault(x => x.Id==typeId);
                if (roomType != null)
                {
                    ReservationPlan.AmountOfRoomTypes.Add(new AmountOfRoomType()
                    {
                        TypeId = roomType.Id,
                        Amount = 1,
                        TypeName = roomType.Name,
                    });
                }
            }
            SavePlanInCookie();
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostRemoveRoom(Guid typeId)
        {
            if (!RestorePlanFromCookie())
                return Page();
            var typeAmount = ReservationPlan.AmountOfRoomTypes.SingleOrDefault(x => x.TypeId == typeId);
            if (typeAmount != null)
            {

                typeAmount.Amount--;
                if (typeAmount.Amount <= 0)
                {
                    ReservationPlan.AmountOfRoomTypes.Remove(typeAmount);
                }
                var validationResult = await reservationPlanValidator.ValidateAsync(ReservationPlan);
                if (validationResult.IsValid)
                {
                    SavePlanInCookie();
                }        
            }
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostBook(Guid typeId)
        {
            if (!RestorePlanFromCookie())
                return Page();

            var validationResult = await reservationPlanValidator.ValidateAsync(ReservationPlan);

            if (!validationResult.IsValid)
            {
                ModelState.AddModelError(validationResult.Errors.First().ErrorCode,validationResult.Errors.First().ErrorMessage);
                return Page();
            }
            if (ReservationPlan.AmountOfRoomTypes.Count() <= 0 )
            { 
                ModelState.AddModelError("Room count","Reservation must contain at least one room");
                return Page();
            }

            try
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    ModelState.AddModelError("User not found","Signed in user was not found");
                    return Page();
                }

                await reservationManager.Book(user,ReservationPlan);
                //Siker
                RedirectToPage("/Account/Manage/Index",new {area = "Identity" });
            } catch (Exception ex)
            {
                ModelState.AddModelError("Booking error",ex.Message);
                return Page();

            }

            SavePlanInCookie(true);
            return RedirectToPage("/Account/Manage/Index",new {area="Identity"});
        }

        public async Task<IActionResult> OnPostCurrency()
        {
            if(RestorePlanFromCookie() && TargetCurrencyCode != null)
            {
				try
				{
                    CurrValue = await currencyConverter.ConvertAsync(TargetCurrencyCode,reservationManager.CalculatePrice(ReservationPlan).Result);
				} catch (Exception ex)
                {
                    _logger.LogError(ex.Message,ex);
                    ModelState.AddModelError("Convert error","Converting currencies is currently not available.");
                }

			}

            return Page();  
        }

        public async Task<IActionResult> OnPostExtra()
        {
            if (RestorePlanFromCookie())
            {
                ReservationPlan.ExtraIds.Clear();
                ReservationPlan.ExtraIds.AddRange(SelectedExtras);
                SavePlanInCookie();
            }
            return RedirectToPage("/Index");
            
        }

        private bool RestorePlanFromCookie()
        {
            var cookieJson = Request.Cookies["ReservationPlan"];
            if ( cookieJson == null) { return false; }
            
            try
            {
                var cookieValue = JsonSerializer.Deserialize<ReservationPlan>(cookieJson);
                if (cookieValue == null) { return false; }

                var result = reservationPlanValidator.Validate(cookieValue);
                if (!result.IsValid)
                {
                    return false;
                }
                ReservationPlan = cookieValue;
                roomTypesAvailabilities = reservationManager.GetAvailabilityAsync(ReservationPlan.StartDate,ReservationPlan.EndDate).Result;
                
                return true;
            } catch (Exception ex)
            {
                return false;
            }
            
        }
        private void SavePlanInCookie(bool empty = false)
        {
            
            CookieOptions options = new CookieOptions();
            options.Secure = true;
            options.HttpOnly = true;
            options.Expires = DateTime.Now.AddMinutes(5);

            if (empty)
            {
                options.Expires = DateTime.Now;
                Response.Cookies.Append("ReservationPlan","",options);
            } else
            {
                Response.Cookies.Append("ReservationPlan",JsonSerializer.Serialize(ReservationPlan),options);
            }

            
        }

        public async Task<List<Extra>> GetExtrasAsync()
        {
            return await context.Extras.ToListAsync();
        }
        public List<string> GetCodes()
        {
            return currencyConverter.GetCodes();
        }

    }
}