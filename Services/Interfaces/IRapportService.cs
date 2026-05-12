namespace ProjetEMIT.Services.Interfaces;

public interface IRapportService
{
    Task<byte[]> GenerateEmploiDuTempsPdfAsync(int? classeId = null, int? enseignantId = null, int? salleId = null, DateOnly? dateDebut = null, DateOnly? dateFin = null);
    Task<byte[]> GenerateRapportOccupationSallesAsync(DateOnly dateDebut, DateOnly dateFin);
}
