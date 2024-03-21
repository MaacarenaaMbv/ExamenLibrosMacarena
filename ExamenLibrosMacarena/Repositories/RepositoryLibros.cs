using ExamenLibrosMacarena.Data;
using ExamenLibrosMacarena.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamenLibrosMacarena.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;
        public RepositoryLibros(LibrosContext context)
        {
            this.context = context; 
        }

        public async Task<List<Libro>> GetLibrosAsync() 
        {
            return await this.context.Libros.ToListAsync();
        }
        public async Task<List<Genero>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }

        public async Task<List<Libro>> GetLibroGeneroAsync(int idgenero)
        {
            return await this.context.Libros.Where(x  => x.IdGenero == idgenero).ToListAsync();
        }

        public async Task<Libro> FindLibroIdAsync(int idLibro)
        {
            return await this.context.Libros.FirstOrDefaultAsync(x => x.IdLibro == idLibro);
        }

        public async Task<Usuario> FindUsuarioAsync(int idusuario)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(x => x.IdUsuario == idusuario);

        }

        public async Task<List<Libro>> GetLibrosSessionAsync(List<int> libros)
        {
            return await this.context.Libros.Where(l => libros.Contains(l.IdLibro)).ToListAsync();
        }

        public async Task<Usuario> LogInUsuarioAsync(string email, string password)
        {
            Usuario usuario = await this.context.Usuarios.Where(z => z.Email == email && z.Pass == password).FirstOrDefaultAsync();
            return usuario;
        }

        public async Task<int> GetMaxIdFacturaAsync()
        {
            return await this.context.Pedidos.MaxAsync(l => l.IdFactura) + 1;
        }

        public async Task<int> GetMaxIdPedidoAsync()
        {
            return await this.context.Pedidos.MaxAsync(l => l.IdPedido) + 1;
        }

        public async Task ComprarCarritoAsync(List<int> carrito, int idusuario)
        {
            int idfactura = await GetMaxIdFacturaAsync();
            DateTime fecha = DateTime.Now;
            foreach (int idlibro in carrito.Distinct())
            {
                int idpedido = await GetMaxIdPedidoAsync();
                await this.context.Pedidos.AddAsync
                    (
                        new Pedido
                        {
                            IdPedido = idpedido,
                            IdFactura = idfactura,
                            IdLibro = idlibro,
                            Fecha = fecha,
                            Cantidad = 1,
                            IdUsuario = idusuario
                        }
                    );
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<List<VistaPedido>> GetPedidosUsuariosAsync(int idusuario)
        {
            return await this.context.VistaPedidos.Where(v => v.IdUsuario == idusuario).ToListAsync();  
        }

    }
}
