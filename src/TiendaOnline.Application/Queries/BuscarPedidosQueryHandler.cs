using TiendaOnline.Application.Interfaces;
using TiendaOnline.Contracts;
using TiendaOnline.Domain.Aggregates;
using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Repositories;

namespace TiendaOnline.Application.Queries
{
    /// <summary>
    /// Manejador que ejecuta la búsqueda de pedidos con los filtros especificados.
    /// </summary>
    public class BuscarPedidosQueryHandler : IQueryHandler<BuscarPedidosQuery, IReadOnlyList<PedidoResumenDTO>>
    {
        private readonly IReadOnlyStore _store;

        /// <summary>
        /// Inicializa una nueva instancia del manejador de búsqueda de pedidos.
        /// </summary>
        /// <param name="store">Almacén de solo lectura.</param>
        public BuscarPedidosQueryHandler(IReadOnlyStore store)
        {
            _store = store;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<PedidoResumenDTO>> HandleAsync(BuscarPedidosQuery query, CancellationToken cancellationToken = default)
        {
            var pedidos = _store.Set<Pedido>();
            var clientes = _store.Set<Cliente>();

            if (query.ClienteId.HasValue)
                pedidos = pedidos.Where(p => p.ClienteId == query.ClienteId.Value);

            if (query.Estado.HasValue)
                pedidos = pedidos.Where(p => p.Estado == query.Estado.Value);

            if (query.FechaDesde.HasValue)
                pedidos = pedidos.Where(p => p.FechaCreacion >= query.FechaDesde.Value);

            if (query.FechaHasta.HasValue)
                pedidos = pedidos.Where(p => p.FechaCreacion <= query.FechaHasta.Value);

            var proyeccion = pedidos
                .Join(clientes,
                    p => p.ClienteId,
                    c => c.Id,
                    (p, c) => new { Pedido = p, ClienteNombre = c.Nombre })
                .Select(x => new PedidoResumenDTO(
                    x.Pedido.Id,
                    x.ClienteNombre,
                    x.Pedido.Estado.ToString(),
                    x.Pedido.FechaCreacion,
                    x.Pedido.Lineas.Count,
                    x.Pedido.Total))
                .OrderByDescending(x => x.FechaCreacion)
                .Skip((query.Pagina - 1) * query.ElementosPorPagina)
                .Take(query.ElementosPorPagina);

            if (query.TotalMinimo.HasValue)
                proyeccion = proyeccion.Where(x => x.Total >= query.TotalMinimo.Value);

            var resultado = await _store.ExecuteAsync(proyeccion, cancellationToken);
            return resultado.AsReadOnly();
        }
    }
}
