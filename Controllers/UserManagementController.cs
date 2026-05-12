using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetEMIT.Models;
using ProjetEMIT.ViewModels;

namespace ProjetEMIT.Controllers;

[Authorize(Roles = "Administrateur")]
public class UserManagementController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var userViewModels = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userViewModels.Add(new UserViewModel
            {
                Id = user.Id,
                Nom = user.Nom,
                Prenom = user.Prenom,
                Email = user.Email ?? "",
                UserName = user.UserName ?? "",
                Roles = roles.ToList(),
                EnseignantId = user.EnseignantId
            });
        }

        return View(userViewModels);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).Where(n => !string.IsNullOrEmpty(n)).ToList()!;
        return View(new CreateUserViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).Where(n => !string.IsNullOrEmpty(n)).ToList()!;
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            Nom = model.Nom,
            Prenom = model.Prenom,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(model.Role))
                await _userManager.AddToRoleAsync(user, model.Role);

            if (model.Role == "Enseignant" && model.EnseignantId.HasValue)
            {
                user.EnseignantId = model.EnseignantId;
                await _userManager.UpdateAsync(user);
            }

            TempData["Success"] = "Utilisateur crùù avec succùs !";
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).Where(n => !string.IsNullOrEmpty(n)).ToList()!;
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> EditRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var model = new EditRolesViewModel
        {
            UserId = user.Id,
            Email = user.Email ?? "",
            CurrentRoles = (await _userManager.GetRolesAsync(user)).ToList()
        };

        ViewBag.AllRoles = _roleManager.Roles.Select(r => r.Name).Where(n => !string.IsNullOrEmpty(n)).ToList()!;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoles(EditRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound();

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (model.SelectedRoles != null && model.SelectedRoles.Count > 0)
            await _userManager.AddToRolesAsync(user, model.SelectedRoles);

        TempData["Success"] = "Rùles mis ù jour avec succùs !";
        return RedirectToAction(nameof(Index));
    }
}
