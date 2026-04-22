using TiendaOnline.Application.Interfaces;
using TiendaOnline.Contracts;
using TiendaOnline.Domain.Aggregates;

namespace TiendaOnline.Application.Queries
{
    /// <summary>
    /// Consulta para buscar pedidos con filtros opcionales y paginación.
    /// </summary>
    public class BuscarPedidosQuery : IQuery<IReadOnlyList<PedidoResumenDTO>>
    {
        /// <summary>
        /// Identificador del cliente para filtrar (opcional).
        /// </summary>
        public int? ClienteId { get; init; }

        /// <summary>
        /// Estado del pedido para filtrar (opcional).
        /// </summary>
        public EstadoPedido? Estado { get; init; }

        /// <summary>
        /// Fecha mínima de creación para filtrar (opcional).
        /// </summary>
        public DateTime? FechaDesde { get; init; }

        /// <summary>
        /// Fecha máxima de creación para filtrar (opcional).
        /// </summary>
        public DateTime? FechaHasta { get; init; }

        /// <summary>
        /// Total mínimo del pedido para filtrar (opcional).
        /// </summary>
        public decimal? TotalMinimo { get; init; }

        /// <summary>
        /// Número de página para la paginación (por defecto 1).
        /// </summary>
        public int Pagina { get; init; } = 1;

        /// <summary>
        /// Cantidad de elementos por página (por defecto 20).
        /// </summary>
        public int ElementosPorPagina { get; init; } = 20;
    }
}
