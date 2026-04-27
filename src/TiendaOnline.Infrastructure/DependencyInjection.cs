using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TiendaOnline.Domain.Repositories;
using TiendaOnline.Infrastructure.Data;
using TiendaOnline.Infrastructure.Repositories;

namespace TiendaOnline.Infrastructure;

/// <summary>
/// Extensiones para configurar la inyección de dependencias de la capa de infraestructura.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de la capa de infraestructura en el contenedor de dependencias.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <param name="connectionString">Cadena de conexión a la base de datos.</param>
    /// <returns>La colección de servicios con los registros añadidos.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TiendaOnlineDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IRepositorioPedido, PedidoRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IProductoRepository, ProductoRepository>();
        services.AddScoped<IReadOnlyStore, ReadOnlyStore>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
