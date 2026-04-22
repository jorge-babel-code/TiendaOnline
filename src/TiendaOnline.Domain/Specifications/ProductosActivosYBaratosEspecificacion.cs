using System.Linq.Expressions;
using TiendaOnline.Domain.Entities;

namespace TiendaOnline.Domain.Specifications
{
    /// <summary>
    /// Especificación que filtra productos activos con precio inferior al máximo especificado.
    /// </summary>
    public class ProductosActivosYBaratosEspecificacion : Especificacion<Producto>
    {
        private readonly decimal _precioMaximo;

        /// <summary>
        /// Crea una nueva especificación para filtrar productos activos y baratos.
        /// </summary>
        /// <param name="precioMaximo">Precio máximo permitido.</param>
        public ProductosActivosYBaratosEspecificacion(decimal precioMaximo)
        {
            _precioMaximo = precioMaximo;
        }

        /// <summary>
        /// Criterio que verifica si el producto está activo y su precio con descuento es menor al máximo.
        /// </summary>
        protected override Expression<Func<Producto, bool>> Criterio =>
            producto => producto.EstaActivo && producto.PrecioConDescuento < _precioMaximo;
    }
}
