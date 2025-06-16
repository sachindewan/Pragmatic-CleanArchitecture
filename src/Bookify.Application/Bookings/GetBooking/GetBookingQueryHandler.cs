using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Abstractions;
using Dapper;
using MediatR;

namespace Bookify.Application.Booking.GetBooking
{
    internal sealed class GetBookingQueryHandler : IRequestHandler<GetBookingQuery, Result<BookingResponse>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            using var connection = _sqlConnectionFactory.CreateConnection();
            connection.Open();
            var query = @"""
                    SELECT 
                          b.id as Id,
                          b.apartment_id as ApartmentId,
                          b.user_id as UserId,
                          b.status as Status, 
                          b.created_on_utc as CreatedOnUtc,
                          b.duration_end as DurationEnd,
                          b.duration_start as DurationStart,
                          b.price_for_period_amount as PriceAmount,
                          b.price_for_period_currency as PriceCurrency,
                          b.cleaning_fee_amount as CleaningFeeAmount,
                          b.cleaning_fee_currency as CleaningFeeCurrency,
                          b.ammenties_up_charge_fee_amount as AmmentiesUpChargeFeeAmount,
                          b.ammenties_up_charge_fee_currency as AmmentiesUpChargeFeeCurrency,
                          b.total_price_amount as TotalPriceAmount,
                          b.total_price_currency as TotalPriceCurrency
                    FROM Bookings b
                    WHERE b.Id = @BookingId""";
            connection.Open();
            var booking = connection.QueryFirstOrDefault<BookingResponse>(
                query,
                new
                {
                    BookingId = request.BookingId
                });
            return booking;
        }
    }
}
