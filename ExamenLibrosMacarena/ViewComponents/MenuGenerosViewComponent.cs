using ExamenLibrosMacarena.Models;
using ExamenLibrosMacarena.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamenLibrosMacarena.ViewComponents
{
    public class MenuGenerosViewComponent: ViewComponent
    {
        private RepositoryLibros repo;

        public MenuGenerosViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Genero> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
