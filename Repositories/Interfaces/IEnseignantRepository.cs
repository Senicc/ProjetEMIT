public interface IEnseignantRepository : IBaseRepository<Enseignant>
{
    Task<Enseignant?> GetWithMatieresAsync(int id);
    Task<IEnumerable<Enseignant>> GetBySpecialiteAsync(string specialite);
}