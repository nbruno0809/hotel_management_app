using FluentValidation;
using HotelManagement.DTO;

namespace HotelManagement.Validator
{
    public class ReservationPlanValidator : AbstractValidator<ReservationPlan>
    {

        public ReservationPlanValidator()
        {
            DateTime now = DateTime.Now;

            RuleFor(x => x.StartDate)
                .GreaterThan(x => now)
                .WithMessage("Please choose a future date")
                .NotEmpty();

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage("Check-out must be after the the check-in date")
                .NotEmpty();

            RuleFor(x => x.AmountOfRoomTypes)
                .NotNull()
                .ForEach(y => y.ChildRules(z =>
                    { 
                        z.RuleFor(w => w.Amount).GreaterThan(0);
                        z.RuleFor(w => w.TypeId).NotEmpty().NotNull().NotEqual(Guid.Empty);
                    }));
        }
    }
}
