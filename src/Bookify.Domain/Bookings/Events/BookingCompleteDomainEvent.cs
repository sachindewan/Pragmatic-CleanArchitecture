using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Bookings.Events
{
    public sealed record BookingCompleteDomainEvent(Guid Id) : IDomainEvent;

}