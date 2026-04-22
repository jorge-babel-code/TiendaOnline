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
}