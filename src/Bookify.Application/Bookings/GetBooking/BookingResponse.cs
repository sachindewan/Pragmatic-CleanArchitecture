using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;

namespace Bookify.Application.Booking.GetBooking
{
    public class BookingResponse
    {
        public Guid BookingId { get; init; }
        public Guid ApartmentId { get; init; }
        public Guid UserId { get; init; }
       
        public decimal PriceAmount { get; init ; }
        public string PriceCurrency { get; init ; }
        public decimal CleaningFeeAmount { get; init ; }
        public string CleaningFeeCurrency { get; init ; }
        public decimal AmmentiesUpChargeFeeAmount { get; init ; }
        public string AmmentiesUpChargeFeeCurrency { get; init ; }
        public decimal TotalPriceAmount { get; init ; }
        public string TotalPriceCurrency { get; init ; }
        public DateOnly DurationStart { get; init ; }
        public DateOnly DurationEnd { get; init ; }
        public int Status { get; init ; }
        public DateTime CreatedOnUtc { get; init ; }
    }
}