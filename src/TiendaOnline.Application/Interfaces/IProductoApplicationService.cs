using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Application.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de aplicación para operaciones de productos.
    /// </summary>
    public interface IProductoApplicationService
    {
        /// <summary>
        /// Obtiene productos activos cuyo precio con descuento sea menor al máximo especificado.
        /// </summary>
        /// <param name="precioMaximo">Precio máximo permitido.</param>
        /// <returns>Lista de productos activos y baratos.</returns>
        Task<List<Producto>> ObtenerProductosActivosYBaratos(decimal precioMaximo);
    }
}
