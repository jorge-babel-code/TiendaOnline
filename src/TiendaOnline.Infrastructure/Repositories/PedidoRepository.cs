using Microsoft.EntityFrameworkCore;
using TiendaOnline.Domain.Aggregates;
using TiendaOnline.Domain.Repositories;
using TiendaOnline.Domain.Specifications;
using TiendaOnline.Infrastructure.Data;

namespace TiendaOnline.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del repositorio de pedidos.
    /// </summary>
    public class PedidoRepository : RepositorioGenerico<Pedido>, IRepositorioPedido
    {
        /// <summary>
        /// Inicializa una nueva instancia del repositorio de pedidos.
        /// </summary>
        /// <param name="context">Contexto de base de datos de la tienda.</param>
        public PedidoRepository(TiendaOnlineDbContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<List<Pedido>> ObtenerConLineasPorEspecificacionAsync(IEspecificacion<Pedido> especificacion)
        {
            return await _conjunto
                .Include(p => p.Lineas)
                .Where(especificacion.AExpresion())
                .ToListAsync();
        }

    }
}
