using TiendaOnline.Domain.Specifications;

/// <summary>
/// Interfaz genérica para operaciones CRUD de repositorio.
/// </summary>
/// <typeparam name="T">Tipo de entidad del repositorio.</typeparam>
public interface IRepositorioGenerico<T> where T : class
{
    /// <summary>
    /// Obtiene entidades que cumplen con la especificación dada.
    /// </summary>
    /// <param name="especificacion">Especificación de filtrado.</param>
    /// <returns>Lista de entidades que cumplen el criterio.</returns>
    Task<List<T>> ObtenerPorEspecificacionAsync(IEspecificacion<T> especificacion);

    /// <summary>
    /// Obtiene una entidad por su identificador.
    /// </summary>
    /// <param name="id">Identificador de la entidad.</param>
    /// <returns>La entidad encontrada o null.</returns>
    Task<T?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Agrega una nueva entidad al repositorio.
    /// </summary>
    /// <param name="entidad">Entidad a agregar.</param>
    void Agregar(T entidad);

    /// <summary>
    /// Actualiza una entidad existente en el repositorio.
    /// </summary>
    /// <param name="entidad">Entidad a actualizar.</param>
    void Actualizar(T entidad);

    /// <summary>
    /// Elimina una entidad del repositorio.
    /// </summary>
    /// <param name="entidad">Entidad a eliminar.</param>
    void Eliminar(T entidad);

    /// <summary>
    /// Guarda todos los cambios pendientes en el almacén de datos.
    /// </summary>
    Task GuardarCambiosAsync();
}