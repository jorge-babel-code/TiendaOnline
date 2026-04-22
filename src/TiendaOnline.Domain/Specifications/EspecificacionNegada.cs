using System.Linq.Expressions;

namespace TiendaOnline.Domain.Specifications
{
    /// <summary>
    /// Especificación que niega otra especificación.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad sobre la que aplica la especificación.</typeparam>
    public sealed class EspecificacionNegada<T> : Especificacion<T>
    {
        private readonly IEspecificacion<T> _especificacion;

        /// <summary>
        /// Crea una nueva especificación negada.
        /// </summary>
        /// <param name="especificacion">Especificación a negar.</param>
        public EspecificacionNegada(IEspecificacion<T> especificacion)
        {
            _especificacion = especificacion;
        }

        /// <summary>
        /// Criterio que niega la especificación original.
        /// </summary>
        protected override Expression<Func<T, bool>> Criterio
        {
            get
            {
                var parametro = Expression.Parameter(typeof(T), "p");
                var cuerpo = Expression.Invoke(_especificacion.AExpresion(), parametro);
                var negacion = Expression.Not(cuerpo);
                return Expression.Lambda<Func<T, bool>>(negacion, parametro);
            }
        }
    }
}
