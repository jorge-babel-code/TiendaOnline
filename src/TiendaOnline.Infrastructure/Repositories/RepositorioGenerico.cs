using Microsoft.EntityFrameworkCore;
using TiendaOnline.Domain.Repositories;
using TiendaOnline.Domain.Specifications;

namespace TiendaOnline.Infrastructure.Repositories;

/// <summary>
/// Implementación genérica del repositorio usando Entity Framework Core.
/// </summary>
/// <typeparam name="T">Tipo de entidad del repositorio.</typeparam>
public class RepositorioGenerico<T> : IRepositorioGenerico<T> where T : class
{
    /// <summary>
    /// Contexto de base de datos.
    /// </summary>
    protected readonly DbContext _contexto;

    /// <summary>
    /// Conjunto de entidades DbSet.
    /// </summary>
    protected readonly DbSet<T> _conjunto;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio genérico.
    /// </summary>
    /// <param name="contexto">Contexto de Entity Framework.</param>
    public RepositorioGenerico(DbContext contexto)
    {
        _contexto = contexto;
        _conjunto = contexto.Set<T>();
    }

    /// <inheritdoc />
    public virtual async Task<List<T>> ObtenerPorEspecificacionAsync(IEspecificacion<T> especificacion)
    {
        return await _conjunto
            .Where(especificacion.AExpresion())
            .ToListAsync();
    }

    /// <inheritdoc />
    public virtual async Task<T?> ObtenerPorIdAsync(int id)
    {
        return await _conjunto.FindAsync(id);
    }

    /// <inheritdoc />
    public virtual void Agregar(T entidad)
    {
        _conjunto.Add(entidad);
    }

    /// <inheritdoc />
    public virtual void Actualizar(T entidad)
    {
        _conjunto.Update(entidad);
    }

    /// <inheritdoc />
    public virtual void Eliminar(T entidad)
    {
        _conjunto.Remove(entidad);
    }

    /// <inheritdoc />
    public virtual async Task GuardarCambiosAsync()
    {
        await _contexto.SaveChangesAsync();
    }
}
