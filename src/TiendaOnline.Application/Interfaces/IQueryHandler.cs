namespace TiendaOnline.Application.Interfaces
{
    /// <summary>
    /// Define el contrato para un manejador de consultas.
    /// </summary>
    /// <typeparam name="TQuery">Tipo de la consulta.</typeparam>
    /// <typeparam name="TResult">Tipo del resultado.</typeparam>
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Ejecuta la consulta y devuelve el resultado.
        /// </summary>
        /// <param name="query">Consulta a ejecutar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Resultado de la consulta.</returns>
        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
    }
}
