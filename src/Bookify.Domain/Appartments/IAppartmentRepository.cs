namespace Bookify.Domain.Appartments
{
    public interface IAppartmentRepository
    {
        Task<Apartment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
