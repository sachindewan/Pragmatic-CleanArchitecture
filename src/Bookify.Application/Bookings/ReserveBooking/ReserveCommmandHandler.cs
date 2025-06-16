﻿using Bookify.Application.Abstractions.Clock;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Appartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Users;
using BookingDomain = Bookify.Domain.Bookings;

namespace Bookify.Application.Booking.ReserveBooking
{
    internal sealed class ReserveCommmandHandler : ICommandHandler<ReserveBookingCommand, Guid>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAppartmentRepository _apartmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PricingService _pricingService;
        private readonly IDateTimeProvider _dateTimeProvider;
        public ReserveCommmandHandler(
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IAppartmentRepository apartmentRepository,
            IUnitOfWork unitOfWork,
            PricingService pricingService,
            IDateTimeProvider dateTimeProvider)
        {
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _apartmentRepository = apartmentRepository;
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Result<Guid>> Handle(ReserveBookingCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            var apartment = await _apartmentRepository.GetByIdAsync(request.ApartmentId, cancellationToken);

            if (apartment is null)
            {
                return Result.Failure<Guid>(ApartmentErrors.NotFound);
            }

            var duration = DateRange.Create(request.StartDate, request.EndDate);

            if (await _bookingRepository.IsOverlappingAsync(apartment, duration, cancellationToken))
            {
                return Result.Failure<Guid>(BookingErrors.Overlap);
            }
            try
            {
                var booking = BookingDomain.Booking.Create(
               apartment,
               user.Id,
               duration,
               _dateTimeProvider.UtcNow,
               _pricingService);

                _bookingRepository.Add(booking);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return booking.Id;
            }
            catch (ConcurrencyException)
            {
                return Result.Failure<Guid>(BookingErrors.Overlap);
            }

        }
    }
}
