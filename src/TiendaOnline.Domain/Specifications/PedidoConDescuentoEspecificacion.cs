using System.Linq.Expressions;
using TiendaOnline.Domain.Aggregates;

namespace TiendaOnline.Domain.Specifications
{
    /// <summary>
    /// Especificación que filtra pedidos que tienen descuento aplicado.
    /// </summary>
    public sealed class PedidoConDescuentoEspecificacion : Especificacion<Pedido>
    {
        /// <summary>
        /// Criterio que verifica si el pedido tiene un descuento asignado.
        /// </summary>
        protected override Expression<Func<Pedido, bool>> Criterio
            => pedido => pedido.Descuento != null;
    }
}
