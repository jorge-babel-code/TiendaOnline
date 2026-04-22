using TiendaOnline.Contracts;
using TiendaOnline.Domain.Aggregates;
using TiendaOnline.SharedKernel;

namespace TiendaOnline.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de aplicación para operaciones de pedidos.
    /// </summary>
    public interface IPedidoApplicationService
    {
        /// <summary>
        /// Crea un nuevo pedido a partir del DTO proporcionado.
        /// </summary>
        /// <param name="dto">Datos para crear el pedido.</param>
        /// <returns>Tupla con el ID del pedido creado y el resultado de validación.</returns>
        Task<(int? PedidoId, ValidationResult Validation)> CrearPedido(CrearPedidoDTO dto);

        /// <summary>
        /// Obtiene todos los pedidos de un cliente específico.
        /// </summary>
        /// <param name="clienteId">Identificador del cliente.</param>
        /// <returns>Lista de pedidos del cliente.</returns>
        Task<List<Pedido>> ObtenerPedidosPorCliente(int clienteId);

        /// <summary>
        /// Obtiene todos los pedidos confirmados pendientes de envío.
        /// </summary>
        /// <returns>Lista de pedidos pendientes de envío.</returns>
        Task<List<Pedido>> ObtenerPedidosPendientesDeEnvio();

        /// <summary>
        /// Obtiene todos los pedidos en estado borrador pendientes de confirmación.
        /// </summary>
        /// <returns>Lista de pedidos pendientes de confirmación.</returns>
        Task<List<Pedido>> ObtenerPedidosPendientesDeConfirmacion();

        /// <summary>
        /// Obtiene los pedidos con descuento de un cliente específico.
        /// </summary>
        /// <param name="clienteId">Identificador del cliente.</param>
        /// <returns>Lista de pedidos con descuento del cliente.</returns>
        Task<List<Pedido>> ObtenerPedidosConDescuentoDeCliente(int clienteId);
    }
}
