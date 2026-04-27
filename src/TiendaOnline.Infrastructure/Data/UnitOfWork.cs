using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using TiendaOnline.Domain.Repositories;

namespace TiendaOnline.Infrastructure.Data
{
    /// <summary>
    /// Implementación de Unit of Work usando Entity Framework Core.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TiendaOnlineDbContext _dbContext;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(TiendaOnlineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
                _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _dbContext.SaveChangesAsync();
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
