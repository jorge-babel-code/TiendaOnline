# TiendaOnline

Solución inicial para una tienda virtual utilizando DDD, .NET 10 y Blazor (Server/Web). Estructura de carpetas y proyectos:

- **src/TiendaOnline.Domain**: Lógica de dominio (entidades, agregados, repositorios, etc.)
- **src/TiendaOnline.Application**: Lógica de aplicación (casos de uso, servicios de aplicación)
- **src/TiendaOnline.Infrastructure**: Implementaciones técnicas (acceso a datos, servicios externos)
- **src/TiendaOnline.Presentation**: Interfaz de usuario con Blazor

## Primeros pasos

1. Agrega referencias entre proyectos según la arquitectura DDD:
   - Application referencia a Domain
   - Infrastructure referencia a Application y Domain
   - Presentation referencia a Application y Domain
2. Implementa entidades, casos de uso y vistas según tus necesidades.

## Compilar y ejecutar

```
dotnet build src/TiendaOnline.Presentation/TiendaOnline.Presentation.csproj
```

```
dotnet run --project src/TiendaOnline.Presentation/TiendaOnline.Presentation.csproj
```

---

Estructura lista para comenzar el desarrollo siguiendo DDD y Blazor Server.