using ProjetEMIT.Data;
using ProjetEMIT.Models;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Repositories.Implementations;

public class FiliereRepository : BaseRepository<Filiere>, IFiliereRepository
{
    public FiliereRepository(ApplicationDbContext context) : base(context) { }
}
