using System.Threading.Tasks;

namespace TiendaOnline.Domain.Repositories
{
    /// <summary>
    /// Contrato para el manejo de transacciones unitarias en la infraestructura de persistencia.
    /// </summary>
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
