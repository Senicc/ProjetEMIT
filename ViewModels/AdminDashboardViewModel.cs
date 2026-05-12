namespace ProjetEMIT.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalSalles { get; set; }
    public int TotalEnseignants { get; set; }
    public int TotalClasses { get; set; }
    public int TotalSeancesAujourdHui { get; set; }
    public double TauxOccupation { get; set; }

    public List<SalleStat> SallesLesPlusUtilisees { get; set; } = new();
}

public class SalleStat
{
    public string NomSalle { get; set; } = string.Empty;
    public int NombreSeances { get; set; }
    public double TauxOccupation { get; set; }
}
