using Bookify.Domain.Appartments;
using Bookify.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Configurations
{
    internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Apartment> builder)
        {
            builder.ToTable("apartments");
            builder.HasKey(apartment => apartment.Id);

            builder.Property(apartment => apartment.Name)
                .HasMaxLength(200)
                .HasConversion(
                    name => name.value,
                    value => new Name(value));

            builder.Property(x => x.Description)
                    .HasMaxLength(2000)
                    .HasConversion(
                    name => name.value,
                    value => new Description(value));
            

           builder.OwnsOne(x => x.Address);

            builder.OwnsOne(x => x.Price, priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                .HasConversion(currency => currency.Code, code => Currency.FromCode(code));
                 
            });

            builder.OwnsOne(apartment => apartment.CleaningFee, feeBuilder =>
            {
                feeBuilder.Property(money => money.Currency)
                           .HasConversion(currency => currency.Code, code => Currency.FromCode(code));

            });

            builder.Property<uint>("Version").IsRowVersion();
        }
    }
}
