namespace TiendaOnline.Contracts
{
    /// <summary>
    /// DTO con el resumen de un pedido para consultas.
    /// </summary>
    /// <param name="PedidoId">Identificador del pedido.</param>
    /// <param name="ClienteNombre">Nombre del cliente que realizó el pedido.</param>
    /// <param name="Estado">Estado actual del pedido.</param>
    /// <param name="FechaCreacion">Fecha y hora de creación del pedido.</param>
    /// <param name="TotalLineas">Cantidad de líneas en el pedido.</param>
    /// <param name="Total">Total del pedido.</param>
    public record PedidoResumenDTO(
        int PedidoId,
        string ClienteNombre,
        string Estado,
        DateTime FechaCreacion,
        int TotalLineas,
        decimal Total);
}
