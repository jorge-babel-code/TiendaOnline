using Microsoft.Extensions.DependencyInjection;
using TiendaOnline.Application.Interfaces;
using TiendaOnline.Application.Queries;
using TiendaOnline.Application.Services;
using TiendaOnline.Contracts;

namespace TiendaOnline.Application;

/// <summary>
/// Extensiones para configurar la inyección de dependencias de la capa de aplicación.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de la capa de aplicación en el contenedor de dependencias.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <returns>La colección de servicios con los registros añadidos.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<PedidoApplicationService>();
        services.AddScoped<IProductoApplicationService, ProductoApplicationService>();
        services.AddScoped<IQueryHandler<BuscarPedidosQuery, IReadOnlyList<PedidoResumenDTO>>, BuscarPedidosQueryHandler>();

        return services;
    }
}
