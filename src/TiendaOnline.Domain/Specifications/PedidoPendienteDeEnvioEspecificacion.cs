using System.Linq.Expressions;
using TiendaOnline.Domain.Aggregates;

namespace TiendaOnline.Domain.Specifications
{
    /// <summary>
    /// Especificación que filtra pedidos confirmados pero aún no enviados.
    /// </summary>
    public sealed class PedidoPendienteDeEnvioEspecificacion : Especificacion<Pedido>
    {
        /// <summary>
        /// Criterio que verifica si el pedido está confirmado y sin fecha de envío.
        /// </summary>
        protected override Expression<Func<Pedido, bool>> Criterio
            => pedido => pedido.Estado == EstadoPedido.Confirmado
                      && pedido.FechaEnvio == null;
    }
}
