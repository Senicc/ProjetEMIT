using System.ComponentModel.DataAnnotations;        // Pour [Required], [Display], etc.
using Microsoft.AspNetCore.Identity;               // Pour Identity
using Microsoft.EntityFrameworkCore;               // Pour DbContext, DbSet
using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;
using ProjetEMIT.Services.Interfaces;
using ProjetEMIT.Repositories.Interfaces;


namespace ProjetEMIT.Models
{
    public class Salle
    {
        public int Id { get; set; }
        public string NomSalle { get; set; } = string.Empty;
        public int Capacite { get; set; }
        public string TypeSalle { get; set; } = string.Empty; // Amphi, TD, TP, Laboratoire, Salle de réunion
        public string Localisation { get; set; } = string.Empty;
        public bool EstDisponible { get; set; } = true;

        // Navigation properties
        public ICollection<Seance> Seances { get; set; } = new List<Seance>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
