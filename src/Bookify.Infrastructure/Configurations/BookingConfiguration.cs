﻿using Bookify.Domain.Appartments;
using Bookify.Domain.Bookings;
using Bookify.Domain.Shared;
using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Bookings", "bookings");

            builder.HasKey(booking => booking.Id);


            builder.OwnsOne(booking => booking.PriceForPeriod, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(booking => booking.CleaningFee, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(booking => booking.AmmentiesUpCharge, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });

            builder.OwnsOne(booking => booking.TotalPrice, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
            });



            builder.OwnsOne(b => b.Duration);

            builder.HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(booking => booking.ApartmentId);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(booking => booking.UserId);

        }
    }
}
