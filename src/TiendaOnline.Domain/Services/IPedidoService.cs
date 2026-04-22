using TiendaOnline.Domain.Aggregates;

namespace TiendaOnline.Domain.Services
{
    /// <summary>
    /// Interfaz del servicio de dominio para operaciones de pedidos.
    /// </summary>
    public interface IPedidoService
    {
        /// <summary>
        /// Crea un nuevo pedido en el sistema.
        /// </summary>
        /// <param name="pedido">Pedido a crear.</param>
        Task CrearPedidoAsync(Pedido pedido);
    }
}
