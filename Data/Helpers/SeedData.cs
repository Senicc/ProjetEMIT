// Helpers/SeedData.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Data;
using ProjetEMIT.Models;

namespace ProjetEMIT.Helpers
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Création des rôles
            string[] roles = { "Administrateur", "ResponsablePedagogique", "Enseignant", "Etudiant" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Création de l'Administrateur principal
            var adminEmail = "admin@emit.edu";
            var admin = await userManager.FindByEmailAsync(adminEmail);

            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Nom = "Admin",
                    Prenom = "EMIT",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Administrateur");
                }
            }

            // Création d'un Responsable Pédagogique
            var respEmail = "responsable@emit.edu";
            var responsable = await userManager.FindByEmailAsync(respEmail);
            if (responsable == null)
            {
                responsable = new ApplicationUser
                {
                    UserName = respEmail,
                    Email = respEmail,
                    Nom = "Diop",
                    Prenom = "Fatou",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(responsable, "Resp@123456");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(responsable, "ResponsablePedagogique");
                }
            }

            // Création de quelques enseignants (liés plus tard)
            await SeedEnseignantsEtDonneesBasiques(context);

            await context.SaveChangesAsync();
            Console.WriteLine("✅ Seed Data terminé avec succès !");
        }

        private static async Task SeedEnseignantsEtDonneesBasiques(ApplicationDbContext context)
        {
            // Filieres
            if (!context.Filieres.Any())
            {
                context.Filieres.AddRange(
                    new Filiere { Code = "INFO", Nom = "Informatique" },
                    new Filiere { Code = "GEST", Nom = "Gestion" },
                    new Filiere { Code = "TELE", Nom = "Télécommunication" }
                );
                await context.SaveChangesAsync();
            }

            // Niveaux
            if (!context.Niveaux.Any())
            {
                context.Niveaux.AddRange(
                    new Niveau { Nom = "L1" }, new Niveau { Nom = "L2" }, new Niveau { Nom = "L3" },
                    new Niveau { Nom = "M1" }, new Niveau { Nom = "M2" }
                );
                await context.SaveChangesAsync();
            }

            // Creneaux horaires
            if (!context.Creneaux.Any())
            {
                context.Creneaux.AddRange(
                    new Creneau { Nom = "Créneau 1", HeureDebut = new TimeOnly(8, 0), HeureFin = new TimeOnly(10, 0) },
                    new Creneau { Nom = "Créneau 2", HeureDebut = new TimeOnly(10, 30), HeureFin = new TimeOnly(12, 30) },
                    new Creneau { Nom = "Créneau 3", HeureDebut = new TimeOnly(13, 30), HeureFin = new TimeOnly(15, 30) },
                    new Creneau { Nom = "Créneau 4", HeureDebut = new TimeOnly(16, 0), HeureFin = new TimeOnly(18, 0) }
                );
                await context.SaveChangesAsync();
            }

            // Salles
            if (!context.Salles.Any())
            {
                context.Salles.AddRange(
                    new Salle { NomSalle = "Amphi A", Capacite = 150, TypeSalle = "Amphi", Localisation = "Bâtiment A", EstDisponible = true },
                    new Salle { NomSalle = "Salle TD-101", Capacite = 40, TypeSalle = "TD", Localisation = "Bâtiment B", EstDisponible = true },
                    new Salle { NomSalle = "Labo Info 1", Capacite = 30, TypeSalle = "Laboratoire", Localisation = "Bâtiment C", EstDisponible = true }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}