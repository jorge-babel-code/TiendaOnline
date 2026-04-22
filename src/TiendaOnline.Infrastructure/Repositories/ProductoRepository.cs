using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Repositories;
using TiendaOnline.Infrastructure.Data;

namespace TiendaOnline.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del repositorio de productos.
    /// </summary>
    public class ProductoRepository : RepositorioGenerico<Producto>, IProductoRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia del repositorio de productos.
        /// </summary>
        /// <param name="context">Contexto de base de datos de la tienda.</param>
        public ProductoRepository(TiendaOnlineDbContext context) : base(context)
        {
        }
    }
}
