using System.Linq.Expressions;

namespace TiendaOnline.Domain.Specifications
{
    /// <summary>
    /// Clase base abstracta para implementar especificaciones con soporte de combinación lógica..
    /// </summary>
    /// <typeparam name="T">Tipo de entidad sobre la que aplica la especificación.</typeparam>
    public abstract class Especificacion<T> : IEspecificacion<T>
    {
        /// <summary>
        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Expresión lambda del criterio.</returns>
        public Expression<Func<T, bool>> AExpresion() => Criterio;

        /// <summary>
        /// Criterio de filtrado definido por las clases derivadas.
        /// </summary>
        protected abstract Expression<Func<T, bool>> Criterio { get; }

        /// <summary>
        /// Combina esta especificación con otra usando operador AND.
        /// </summary>
        /// <param name="otra">Otra especificación a combinar.</param>
        /// <returns>Nueva especificación combinada.</returns>
         public Especificacion<T> Y(IEspecificacion<T> otra) => new EspecificacionCombinada<T>(this, otra, ExpressionType.AndAlso);

        /// <summary>
        /// Combina esta especificación con otra usando operador OR.
        /// </summary>
        /// <param name="otra">Otra especificación a combinar.</param>
        /// <returns>Nueva especificación combinada.</returns>
        public Especificacion<T> O(IEspecificacion<T> otra) => new EspecificacionCombinada<T>(this, otra, ExpressionType.OrElse);

        /// <summary>
        /// Niega esta especificación.
        /// </summary>
        /// <returns>Nueva especificación negada.</returns>
        public Especificacion<T> No() => new EspecificacionNegada<T>(this);
    }
}

