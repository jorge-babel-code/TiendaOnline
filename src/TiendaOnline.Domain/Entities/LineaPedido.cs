using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.ValueObjects;

namespace TiendaOnline.Domain.Entities
{
    /// <summary>
    /// Representa una línea individual dentro de un pedido.
    /// </summary>
    public class LineaPedido
    {
        /// <summary>
        /// Identificador único de la línea de pedido.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Identificador del producto asociado.
        /// </summary>
        public int ProductoId { get; private set; }

        /// <summary>
        /// Nombre del producto al momento de crear la línea.
        /// </summary>
        public string ProductoNombre { get; private set; }

        /// <summary>
        /// Cantidad de unidades del producto.
        /// </summary>
        public Cantidad Cantidad { get; private set; }

        /// <summary>
        /// Precio unitario del producto al momento de crear la línea.
        /// </summary>
        public Precio PrecioUnitario { get; private set; }

        /// <summary>
        /// Subtotal calculado (cantidad x precio unitario).
        /// </summary>
        public decimal Subtotal => Cantidad.Value * PrecioUnitario.Value;

        /// <summary>
        /// Constructor interno para crear una línea de pedido.
        /// </summary>
        /// <param name="productoId">Identificador del producto.</param>
        /// <param name="productoNombre">Nombre del producto.</param>
        /// <param name="cantidad">Cantidad de unidades.</param>
        /// <param name="precioUnitario">Precio por unidad.</param>
        internal LineaPedido(int productoId, string productoNombre, Cantidad cantidad, Precio precioUnitario)
        {
            ProductoId = productoId;
            ProductoNombre = productoNombre;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }
    }
}
