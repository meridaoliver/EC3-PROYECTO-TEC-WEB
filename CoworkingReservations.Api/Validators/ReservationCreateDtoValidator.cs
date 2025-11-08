using FluentValidation;
using CoworkingReservations.Core.DTOs;

namespace CoworkingReservations.Api.Validators
{
    public class ReservationCreateDtoValidator : AbstractValidator<ReservationCreateDto>
    {
        public ReservationCreateDtoValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.SpaceId).GreaterThan(0);
            RuleFor(x => x.StartDateTime).LessThan(x => x.EndDateTime).WithMessage("Start must be before end");
            RuleFor(x => x).Must(x => (x.EndDateTime - x.StartDateTime) <= TimeSpan.FromHours(8))
                .WithMessage("Duration cannot exceed 8 hours");
        }
    }
}
