using System.Collections.Generic;

namespace TiendaOnline.Contracts
{
    /// <summary>
    /// DTO para crear un nuevo pedido.
    /// </summary>
    public class CrearPedidoDTO
    {
        /// <summary>
        /// Identificador del cliente que realiza el pedido.
        /// </summary>
        public int ClienteId { get; set; }

        /// <summary>
        /// Líneas de productos a incluir en el pedido.
        /// </summary>
        public List<LineaPedidoDTO> Lineas { get; set; } = new();

        /// <summary>
        /// Descuento opcional a aplicar al pedido.
        /// </summary>
        public decimal? Descuento { get; set; }

        /// <summary>
        /// Dirección de envío opcional. Si no se especifica, se usará la del cliente.
        /// </summary>
        public DireccionDTO? DireccionEnvio { get; set; }
    }
}
