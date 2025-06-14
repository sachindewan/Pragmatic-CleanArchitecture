﻿using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Appartments
{
    public static class ApartmentErrors
    {
        public static Error NotFound = new(
            "Apartment.NotFound",
            "The apartment with the specified identifier was not found");
    }
}
