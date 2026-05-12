using System.ComponentModel.DataAnnotations;        // Pour [Required], [Display], etc.
using Microsoft.AspNetCore.Identity;               // Pour Identity
using Microsoft.EntityFrameworkCore;               // Pour DbContext, DbSet
using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Models
{
    public class Seance
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int CreneauId { get; set; }
        public Creneau Creneau { get; set; } = null!;

        public int SalleId { get; set; }
        public Salle Salle { get; set; } = null!;

        public int EnseignantId { get; set; }
        public Enseignant Enseignant { get; set; } = null!;

        public int MatiereId { get; set; }
        public Matiere Matiere { get; set; } = null!;

        public int ClasseId { get; set; }
        public Classe Classe { get; set; } = null!;

        public string TypeSeance { get; set; } = "Cours"; // Cours, TD, TP, Examen
    }
}
