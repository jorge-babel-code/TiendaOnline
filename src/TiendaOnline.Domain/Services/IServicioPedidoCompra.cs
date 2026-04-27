using TiendaOnline.Domain.Aggregates;
using TiendaOnline.Domain.ValueObjects;
using TiendaOnline.SharedKernel;

namespace TiendaOnline.Domain.Services
{
    /// <summary>
    /// Interfaz del servicio de dominio para orquestar la creación y validación de pedidos.
    /// Contiene lógica de negocio compleja que requiere consultar múltiples agregados.
    /// </summary>
    public interface IServicioPedidoCompra
    {
        /// <summary>
        /// Valida la disponibilidad de stock y calcula el precio final considerando
        /// descuentos por volumen, cliente VIP y promociones activas.
        /// </summary>
        Task<ValidationResult> CrearPedidoConValidacionCompleta(
            int clienteId,
            List<(int productoId, int cantidad)> items,
            Descuento? descuentoAplicado);

        /// <summary>
        /// Procesa una devolución validando el plazo y actualizando el stock.
        /// </summary>
        Task<ValidationResult> ProcesarDevolucionConActualizacionStock(
            int pedidoId,
            List<(int productoId, int cantidad)> itemsADevolver);
    }
}
