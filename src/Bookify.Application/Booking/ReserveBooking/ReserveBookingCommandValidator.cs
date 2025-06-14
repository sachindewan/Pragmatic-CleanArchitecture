using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Booking.ReserveBooking
{
    public class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
    {
        public ReserveBookingCommandValidator()
        {
            RuleFor(c=>c.UserId).NotEmpty().WithMessage("User ID must not be empty.");
            RuleFor(c => c.ApartmentId).NotEmpty().WithMessage("Apartment ID must not be empty.");
            RuleFor(c => c.StartDate)
                .NotEmpty().WithMessage("Start date must not be empty.")
                .LessThan(c => c.EndDate).WithMessage("Start date must be before end date.");
        }
    }
}
