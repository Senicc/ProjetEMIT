using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Repositories.Implementations;

public class CreneauRepository : BaseRepository<Creneau>, ICreneauRepository
{
    public CreneauRepository(ApplicationDbContext context) : base(context) { }
}
