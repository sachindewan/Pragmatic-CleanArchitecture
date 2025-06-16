using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Abstractions;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Apartments.SearchApartment
{
    internal sealed class SearchApartmentQueryHandler : IRequestHandler<SearchApartmentQuery, Result<IReadOnlyList<ApartmentResponse>>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public SearchApartmentQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public static readonly int[] ActiveBookingStatuses = new int[]
        {
            (int)Domain.Bookings.BookingStatus.Reserved,
            (int)Domain.Bookings.BookingStatus.Confirmed,
            (int)Domain.Bookings.BookingStatus.Completed
        };

        public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentQuery request, CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
            {
                return new List<ApartmentResponse>();
            }
            using var connection = _sqlConnectionFactory.CreateConnection();
            var query = new StringBuilder(@"
                    SELECT 
                        a.id as Id,
                        a.name as Name,
                        a.description as Description,
                        a.price_amount as Price,
                        a.price_currency as Currency,
                        a.address_country as Country,
                        a.address_state as State,
                        a.address_zip_code as ZipCode,
                        a.address_city as City,
                        a.address_street as Street
                    FROM Apartments a
                    WHERE NOT EXISTS (
                        SELECT 1 
                        FROM Bookings b 
                        WHERE b.apartment_id = a.id 
                            AND (b.duration_start < @EndDate AND b.duration_end > @StartDate)
                            AND b.status = ANY(@ActiveBookingStatuses)
                    )");

            var apartmentList = await connection.QueryAsync<ApartmentResponse, AddressResponce, ApartmentResponse>(
                query.ToString(),
                (apartment, address) =>
                {
                    return new ApartmentResponse
                    {
                        Id = apartment.Id,
                        Name = apartment.Name,
                        Description = apartment.Description,
                        Price = apartment.Price,
                        Currency = apartment.Currency,
                        Address = new AddressResponce
                        {
                            Country = address.Country,
                            City = address.City,
                            Street = address.Street,
                            ZipCode = address.ZipCode,
                            State = address.State
                        }
                    };
                },
                new
                {
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    ActiveBookingStatuses
                },
                splitOn:"Country");

            return apartmentList.ToList();
        }
    }
}
