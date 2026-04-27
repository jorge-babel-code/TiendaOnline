using TiendaOnline.Contracts;
using TiendaOnline.SharedKernel;

namespace TiendaOnline.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de aplicación para crear pedidos.
    /// Orquesta el caso de uso de creación de pedidos.
    /// </summary>
    public interface ICrearPedidoApplicationService
    {
        /// <summary>
        /// Crea un nuevo pedido a partir de un DTO.
        /// Delega la validación de negocio al Servicio de Dominio.
        /// </summary>
        /// <param name="comando">Datos del pedido a crear.</param>
        /// <returns>Resultado de la operación con ID del pedido creado.</returns>
        Task<(int? PedidoId, ValidationResult Resultado)> Handle(CrearPedidoDTO comando);
    }
}
