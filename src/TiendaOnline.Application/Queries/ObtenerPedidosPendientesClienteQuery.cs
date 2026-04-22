using TiendaOnline.Application.Interfaces;
using TiendaOnline.Contracts;

namespace TiendaOnline.Application.Queries
{
    /// <summary>
    /// Consulta para obtener los pedidos pendientes de envío de un cliente.
    /// </summary>
    public class ObtenerPedidosPendientesClienteQuery : IQuery<IReadOnlyList<PedidoResumenDTO>>
    {
        /// <summary>
        /// Identificador del cliente.
        /// </summary>
        public int ClienteId { get; init; }
    }
}
