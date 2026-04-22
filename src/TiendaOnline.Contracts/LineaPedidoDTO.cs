namespace TiendaOnline.Contracts
{
    /// <summary>
    /// DTO que representa una línea de pedido.
    /// </summary>
    public class LineaPedidoDTO
    {
        /// <summary>
        /// Identificador del producto.
        /// </summary>
        public int ProductoId { get; set; }

        /// <summary>
        /// Cantidad de unidades del producto.
        /// </summary>
        public int Cantidad { get; set; }

        /// <summary>
        /// Precio unitario del producto.
        /// </summary>
        public decimal PrecioUnitario { get; set; }
    }
}
