using System.Linq.Expressions;
using TiendaOnline.Domain.Aggregates;

namespace TiendaOnline.Domain.Specifications
{
    /// <summary>
    /// Especificación que filtra pedidos que tienen al menos una línea.
    /// </summary>
    public sealed class PedidoConLineasEspecificacion : Especificacion<Pedido>
    {
        /// <summary>
        /// Criterio que verifica si el pedido tiene líneas.
        /// </summary>
        protected override Expression<Func<Pedido, bool>> Criterio
            => pedido => pedido.Lineas.Any();
    }
}