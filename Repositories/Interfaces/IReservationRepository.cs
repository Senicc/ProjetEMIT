using ProjetEMIT.Models;

namespace ProjetEMIT.Repositories.Interfaces;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetAllWithDetailsAsync();
    Task<IEnumerable<Reservation>> GetByDemandeurIdAsync(string demandeurId);
    Task<bool> ChevaucheAsync(int salleId, DateOnly date, TimeOnly debut, TimeOnly fin, int? excludeReservationId = null);
    Task<bool> ExistsActiveForSalleAsync(int salleId);
}
