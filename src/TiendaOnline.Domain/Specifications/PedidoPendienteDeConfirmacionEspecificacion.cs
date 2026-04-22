using System.Linq.Expressions;
using TiendaOnline.Domain.Aggregates;

namespace TiendaOnline.Domain.Specifications;

/// <summary>
/// Especificación que filtra pedidos en estado Borrador (pendientes de confirmación).
/// </summary>
public sealed class PedidoPendienteDeConfirmacionEspecificacion : Especificacion<Pedido>
{
    /// <summary>
    /// Criterio que verifica si el pedido está en estado Borrador.
    /// </summary>
    protected override Expression<Func<Pedido, bool>> Criterio
        => pedido => pedido.Estado == EstadoPedido.Borrador;
}