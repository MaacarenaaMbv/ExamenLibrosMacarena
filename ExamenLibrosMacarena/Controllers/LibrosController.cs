using ExamenLibrosMacarena.Extensions;
using ExamenLibrosMacarena.Filters;
using ExamenLibrosMacarena.Models;
using ExamenLibrosMacarena.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExamenLibrosMacarena.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;

        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Libro> libros = await this.repo.GetLibrosAsync();
            return View(libros);
        }

        public async Task<IActionResult> DetailsLibro(int idlibro)
        {
            Libro libro =  await this.repo.FindLibroIdAsync(idlibro);
            return View(libro);
        }
        public async Task<IActionResult> LibrosGenero(int idgenero)
        {
            List<Libro> libro = await this.repo.GetLibroGeneroAsync(idgenero);
            return View(libro);
        }


        /************* USUARIO *********************/

        [AuthorizeUsuarios]
        public IActionResult PerfilUsuario()
        {
            return View();
        }

        /************** CARRITO *****************/
        public IActionResult AnyadirLibroCarrito(int? idlibro)
        {
            if (idlibro != null)
            {
                List<int> carrito;
                if (HttpContext.Session.GetString("CARRITO") == null)
                {
                    carrito = new List<int>();
                }
                else
                {
                    carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                }
                carrito.Add(idlibro.Value);
                HttpContext.Session.SetObject("CARRITO", carrito);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Carrito()
        {
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            if (carrito != null)
            {
                List<Libro> libros = await this.repo.GetLibrosSessionAsync(carrito);
                return View(libros);
            }
            return View();
        }

        public async Task<IActionResult> EliminarLibroCarrito(int? idlibro)
        {
            if (idlibro != null)
            {
                List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
                carrito.Remove(idlibro.Value);
                if (carrito.Count() == 0)
                {
                    HttpContext.Session.Remove("CARRITO");
                }
                else
                {
                    HttpContext.Session.SetObject("CARRITO", carrito);
                }
            }
            return RedirectToAction("CARRITO");

        }
        [AuthorizeUsuarios]
        public async Task<IActionResult> ComprarCarrito()
        {
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<int> carrito = HttpContext.Session.GetObject<List<int>>("CARRITO");
            await this.repo.ComprarCarritoAsync(carrito, idusuario);
            HttpContext.Session.Remove("CARRITO");
            return RedirectToAction("PedidosUsuario");
        }
        [AuthorizeUsuarios]
        public async Task<IActionResult> PedidosUsuario()
        {
            int idusuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            List<VistaPedido> pedidos = await this.repo.GetPedidosUsuariosAsync(idusuario);
            return View(pedidos);
        }

    }
}
