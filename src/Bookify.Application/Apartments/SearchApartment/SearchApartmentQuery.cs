using Bookify.Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Apartments.SearchApartment
{
    public sealed record SearchApartmentQuery(
        DateOnly StartDate,
        DateOnly EndDate) : IQuery<IReadOnlyList<ApartmentResponse>>;
}

