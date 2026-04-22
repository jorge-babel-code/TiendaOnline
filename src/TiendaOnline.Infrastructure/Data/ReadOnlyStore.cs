using Microsoft.EntityFrameworkCore;
using TiendaOnline.Domain.Repositories;

namespace TiendaOnline.Infrastructure.Data
{
    public class ReadOnlyStore : IReadOnlyStore
    {
        private readonly TiendaOnlineDbContext _context;

        public ReadOnlyStore(TiendaOnlineDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Set<T>() where T : class
        {
            return _context.Set<T>().AsNoTracking();
        }

        public async Task<List<TResult>> ExecuteAsync<TResult>(
            IQueryable<TResult> query,
            CancellationToken cancellationToken = default)
        {
            return await query.ToListAsync(cancellationToken);
        }
    }
}
