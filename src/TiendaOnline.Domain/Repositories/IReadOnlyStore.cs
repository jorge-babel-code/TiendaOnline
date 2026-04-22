namespace TiendaOnline.Domain.Repositories
{
    /// <summary>
    /// Interfaz para acceso de solo lectura al almacén de datos.
    /// </summary>
    public interface IReadOnlyStore
    {
        /// <summary>
        /// Obtiene un conjunto de entidades como IQueryable para consultas.
        /// </summary>
        /// <typeparam name="T">Tipo de entidad.</typeparam>
        /// <returns>IQueryable de la entidad especificada.</returns>
        IQueryable<T> Set<T>() where T : class;

        /// <summary>
        /// Ejecuta una consulta y devuelve los resultados.
        /// </summary>
        /// <typeparam name="TResult">Tipo del resultado.</typeparam>
        /// <param name="query">Consulta a ejecutar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de resultados.</returns>
        Task<List<TResult>> ExecuteAsync<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken = default);
    }
}
