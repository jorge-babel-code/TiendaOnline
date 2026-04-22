using TiendaOnline.Application.Interfaces;
using TiendaOnline.Contracts;
using TiendaOnline.Domain.Aggregates;
using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Repositories;

namespace TiendaOnline.Application.Queries
{
    /// <summary>
    /// Manejador que obtiene los pedidos pendientes de envío de un cliente.
    /// </summary>
    public class ObtenerPedidosPendientesClienteQueryHandler
        : IQueryHandler<ObtenerPedidosPendientesClienteQuery, IReadOnlyList<PedidoResumenDTO>>
    {
        private readonly IReadOnlyStore _store;

        /// <summary>
        /// Inicializa una nueva instancia del manejador.
        /// </summary>
        /// <param name="store">Almacén de solo lectura.</param>
        public ObtenerPedidosPendientesClienteQueryHandler(IReadOnlyStore store)
        {
            _store = store;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<PedidoResumenDTO>> HandleAsync(
            ObtenerPedidosPendientesClienteQuery query,
            CancellationToken cancellationToken = default)
        {
            var pedidos = _store.Set<Pedido>()
                .Where(p => p.ClienteId == query.ClienteId
                         && p.Estado == EstadoPedido.Confirmado
                         && p.FechaEnvio == null);

            var clientes = _store.Set<Cliente>();

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
                .OrderByDescending(x => x.FechaCreacion);

            var resultado = await _store.ExecuteAsync(proyeccion, cancellationToken);
            return resultado.AsReadOnly();
        }
    }
}
