namespace TiendaOnline.Domain.Aggregates
{
    /// <summary>
    /// Estados posibles del ciclo de vida de un pedido.
    /// </summary>
    public enum EstadoPedido
    {
        /// <summary>
        /// Pedido en edición, aún no confirmado.
        /// </summary>
        Borrador,

        /// <summary>
        /// Pedido confirmado y listo para ser enviado.
        /// </summary>
        Confirmado,

        /// <summary>
        /// Pedido enviado al cliente.
        /// </summary>
        Enviado,

        /// <summary>
        /// Pedido entregado al cliente.
        /// </summary>
        Entregado,

        /// <summary>
        /// Pedido cancelado.
        /// </summary>
        Cancelado
    }
}
