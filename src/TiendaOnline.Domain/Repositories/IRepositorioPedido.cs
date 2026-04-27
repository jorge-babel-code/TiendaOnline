using TiendaOnline.Domain.Specifications;
using TiendaOnline.Domain.Aggregates;

/// <summary>
/// Interfaz de repositorio especializada para pedidos.
/// </summary>
public interface IRepositorioPedido : IRepositorioGenerico<Pedido>
{
    /// <summary>
    /// Obtiene pedidos con sus líneas cargadas que cumplen la especificación.
    /// </summary>
    /// <param name="especificacion">Especificación de filtrado.</param>
    /// <returns>Lista de pedidos con líneas incluidas.</returns>
    Task<List<Pedido>> ObtenerConLineasPorEspecificacionAsync(IEspecificacion<Pedido> especificacion);

    /// <summary>
    /// Obtiene el último pedido creado por un cliente.
    /// </summary>
    /// <param name="clienteId">ID del cliente.</param>
    /// <returns>El último pedido del cliente o null si no existe.</returns>
    Task<Pedido?> ObtenerUltimoPedidoDelClienteAsync(int clienteId);
}