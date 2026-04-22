using System.Linq.Expressions;
using TiendaOnline.Domain.Aggregates;

namespace TiendaOnline.Domain.Specifications
{
    /// <summary>
    /// Especificación que filtra pedidos por cliente.
    /// </summary>
    public sealed class PedidoClienteEspecificacion : Especificacion<Pedido>
    {
        private readonly int _clienteId;

        /// <summary>
        /// Crea una nueva especificación para filtrar pedidos de un cliente específico.
        /// </summary>
        /// <param name="clienteId">Identificador del cliente.</param>
        public PedidoClienteEspecificacion(int clienteId)
        {
            _clienteId = clienteId;
        }

        /// <summary>
        /// Criterio que filtra por el identificador del cliente.
        /// </summary>
        protected override Expression<Func<Pedido, bool>> Criterio
            => pedido => pedido.ClienteId == _clienteId;
    }
}
