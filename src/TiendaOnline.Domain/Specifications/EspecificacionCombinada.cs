
using System.Linq.Expressions;

namespace TiendaOnline.Domain.Specifications;

/// <summary>
/// Especificación que combina dos especificaciones usando un operador lógico.
/// </summary>
/// <typeparam name="T">Tipo de entidad sobre la que aplica la especificación.</typeparam>
public sealed class EspecificacionCombinada<T> : Especificacion<T>
{
private readonly IEspecificacion<T> _izquierda;
private readonly IEspecificacion<T> _derecha;
private readonly ExpressionType _operador;

/// <summary>
/// Crea una nueva especificación combinada.
/// </summary>
/// <param name="izquierda">Especificación del lado izquierdo.</param>
/// <param name="derecha">Especificación del lado derecho.</param>
/// <param name="operador">Tipo de operador lógico (AndAlso u OrElse).</param>
public EspecificacionCombinada(IEspecificacion<T> izquierda, IEspecificacion<T> derecha, ExpressionType operador)
{
_izquierda = izquierda;
_derecha = derecha;
_operador = operador;
}

    /// <summary>
    /// Criterio que combina las dos especificaciones con el operador especificado.
    /// </summary>
    protected override Expression<Func<T, bool>> Criterio 
    {
    get     
        {
            var parametro = Expression.Parameter(typeof(T), "p");
            var cuerpoIzq = Expression.Invoke(_izquierda.AExpresion(), parametro);
            var cuerpoDer = Expression.Invoke(_derecha.AExpresion(), parametro);

            var cuerpo = Expression.MakeBinary(_operador, cuerpoIzq, cuerpoDer);
            return Expression.Lambda<Func<T, bool>>(cuerpo, parametro);
        }
    }
}