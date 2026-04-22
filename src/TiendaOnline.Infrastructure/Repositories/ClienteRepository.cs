using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Repositories;
using TiendaOnline.Infrastructure.Data;

namespace TiendaOnline.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación del repositorio de clientes.
    /// </summary>
    public class ClienteRepository : RepositorioGenerico<Cliente>, IClienteRepository
    {
        /// <summary>
        /// Inicializa una nueva instancia del repositorio de clientes.
        /// </summary>
        /// <param name="context">Contexto de base de datos de la tienda.</param>
        public ClienteRepository(TiendaOnlineDbContext context) : base(context)
        {
        }
    }
}
