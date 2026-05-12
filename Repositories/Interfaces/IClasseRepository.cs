using ProjetEMIT.Models;

namespace ProjetEMIT.Repositories.Interfaces;

public interface IClasseRepository : IBaseRepository<Classe>
{
    Task<IEnumerable<Classe>> GetAllWithDetailsAsync();
}
