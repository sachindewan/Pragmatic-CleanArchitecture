using Bookify.Domain.Appartments;

namespace Bookify.Infrastructure.Repositories;

internal sealed class ApartmentRepository : Repository<Apartment>,IAppartmentRepository
{
    public ApartmentRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}