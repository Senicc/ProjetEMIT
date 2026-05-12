using System.ComponentModel.DataAnnotations;        // Pour [Required], [Display], etc.
using Microsoft.AspNetCore.Identity;               // Pour Identity
using Microsoft.EntityFrameworkCore;               // Pour DbContext, DbSet
using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.Repositories.Interfaces;

namespace ProjetEMIT.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int SalleId { get; set; }
        public Salle Salle { get; set; } = null!;

        public string DemandeurId { get; set; } = string.Empty;
        public ApplicationUser Demandeur { get; set; } = null!;

        public DateOnly Date { get; set; }
        public TimeOnly HeureDebut { get; set; }
        public TimeOnly HeureFin { get; set; }

        public string Motif { get; set; } = string.Empty;
        public string Statut { get; set; } = "EnAttente"; // EnAttente, Validee, Refusee, Annulee
        public DateTime DateDemande { get; set; } = DateTime.UtcNow;
    }
}
