using TiendaOnline.Application.Interfaces;
using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Repositories;
using TiendaOnline.Domain.Specifications;

namespace TiendaOnline.Application.Services
{
    /// <summary>
    /// Servicio de aplicación que gestiona las operaciones relacionadas con productos.
    /// </summary>
    public class ProductoApplicationService : IProductoApplicationService
    {
        private readonly IProductoRepository _productoRepository;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de productos.
        /// </summary>
        /// <param name="productoRepository">Repositorio de productos.</param>
        public ProductoApplicationService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        /// <inheritdoc />
        public async Task<List<Producto>> ObtenerProductosActivosYBaratos(decimal precioMaximo)
        {
            var especificacion = new ProductosActivosYBaratosEspecificacion(precioMaximo);
            return await _productoRepository.ObtenerPorEspecificacionAsync(especificacion);
        }
    }
}
