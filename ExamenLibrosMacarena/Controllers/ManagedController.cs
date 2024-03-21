using ExamenLibrosMacarena.Models;
using ExamenLibrosMacarena.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamenLibrosMacarena.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;

        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario usuario = await this.repo.LogInUsuarioAsync(email, password);
            if (usuario != null)
            {
                ClaimsIdentity identity =
                new ClaimsIdentity(
                CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimTypes.Name, ClaimTypes.Role);

                Claim claimName = new Claim(ClaimTypes.Name, usuario.Nombre);
                identity.AddClaim(claimName);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                identity.AddClaim(claimId);
                Claim claimEmail = new Claim("Email", usuario.Email);
                identity.AddClaim(claimEmail);
                Claim claimApellido = new Claim("Apellidos", usuario.Apellidos);
                identity.AddClaim(claimApellido);
                Claim claimFoto = new Claim("Foto", usuario.Foto);
                identity.AddClaim(claimFoto);

                ClaimsPrincipal userPrincipal =
                new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal);

                return RedirectToAction("PerfilUsuario", "Libros");
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Libros");
        }

    }
}
