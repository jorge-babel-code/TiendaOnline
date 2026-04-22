using System.Linq.Expressions;

namespace TiendaOnline.Domain.Specifications
{
    /// <summary>
    /// Define el contrato para una especificación que encapsula criterios de filtrado.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad sobre la que aplica la especificación.</typeparam>
    public interface IEspecificacion<T>
    {
        /// <summary>
        /// Convierte la especificación en una expresión lambda evaluable.
        /// </summary>
        /// <returns>Expresión lambda que representa el criterio de filtrado.</returns>
        Expression<Func<T, bool>> AExpresion();
    }
}
