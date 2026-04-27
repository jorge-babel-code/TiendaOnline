using TiendaOnline.Application.Interfaces;
using TiendaOnline.Contracts;
using TiendaOnline.Domain.Services;
using TiendaOnline.Domain.Repositories;
using TiendaOnline.Domain.ValueObjects;
using TiendaOnline.SharedKernel;

namespace TiendaOnline.Application.Services
{
    /// <summary>
    /// Servicio de Aplicación que orquesta la creación de pedidos.
    /// 
    /// Responsabilidades (capa de aplicación):
    /// - Mapeo de DTOs a entidades de dominio
    /// - Validación de entrada
    /// - Manejo de transacciones
    /// - Delegación de lógica de negocio al Servicio de Dominio
    /// - Manejo de excepciones
    /// </summary>
    public class CrearPedidoApplicationService : ICrearPedidoApplicationService
    {
        private readonly IServicioPedidoCompra _servicioPedidoCompra;
        private readonly IRepositorioPedido _pedidoRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Inicializa una nueva instancia del servicio.
        /// </summary>
        /// <param name="servicioPedidoCompra">Servicio de dominio con la lógica de negocio.</param>
        /// <param name="pedidoRepository">Repositorio de pedidos para obtener el pedido creado.</param>
        public CrearPedidoApplicationService(
            IServicioPedidoCompra servicioPedidoCompra,
            IRepositorioPedido pedidoRepository,
            IUnitOfWork unitOfWork)
        {
            _servicioPedidoCompra = servicioPedidoCompra;
            _pedidoRepository = pedidoRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Ejecuta el caso de uso: crear un pedido.
        /// 
        /// Flujo:
        /// 1. Mapea el DTO a los parámetros del Servicio de Dominio
        /// 2. DELEGA la lógica de negocio al Servicio de Dominio
        /// 3. Si hay errores, retorna el resultado de validación
        /// 4. Si es exitoso, obtiene el ID del pedido creado
        /// 5. Retorna el resultado
        /// </summary>
        public async Task<(int? PedidoId, ValidationResult Resultado)> Handle(CrearPedidoDTO comando)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Validación básica de entrada
                var validation = ValidarDTO(comando);
                if (!validation.IsValid)
                {
                    await _unitOfWork.RollbackAsync();
                    return (null, validation);
                }

                // Convertir LineaPedidoDTO a tuplas (productoId, cantidad)
                var items = comando.Lineas
                    .Select(l => (productoId: l.ProductoId, cantidad: l.Cantidad))
                    .ToList();

                // Convertir Descuento si existe
                Descuento? descuento = null;
                if (comando.Descuento.HasValue && comando.Descuento.Value > 0)
                {
                    var (desc, descValidation) = Descuento.Create(comando.Descuento.Value);
                    if (!descValidation.IsValid)
                    {
                        await _unitOfWork.RollbackAsync();
                        return (null, descValidation);
                    }
                    descuento = desc;
                }

                // DELEGACIÓN al Servicio de Dominio
                var resultado = await _servicioPedidoCompra.CrearPedidoConValidacionCompleta(
                    clienteId: comando.ClienteId,
                    items: items,
                    descuentoAplicado: descuento);

                if (!resultado.IsValid)
                {
                    await _unitOfWork.RollbackAsync();
                    return (null, resultado);
                }

                await _unitOfWork.CommitAsync();

                // Obtener el ID del último pedido creado
                var ultimoPedido = await _pedidoRepository.ObtenerUltimoPedidoDelClienteAsync(comando.ClienteId);

                return (ultimoPedido?.Id, resultado);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                var resultado = ValidationResult.WithError("Error", $"Error al crear pedido: {ex.Message}");
                return (null, resultado);
            }
        }

        /// <summary>
        /// Valida los datos de entrada del DTO.
        /// </summary>
        private ValidationResult ValidarDTO(CrearPedidoDTO comando)
        {
            var validation = new ValidationResult();

            if (comando == null)
            {
                validation.AddError("comando", "El comando no puede ser nulo.");
                return validation;
            }

            if (comando.ClienteId <= 0)
            {
                validation.AddError(nameof(comando.ClienteId), "El ID del cliente debe ser mayor a 0.");
            }

            if (comando.Lineas == null || comando.Lineas.Count == 0)
            {
                validation.AddError(nameof(comando.Lineas), "El pedido debe contener al menos una línea.");
            }
            else
            {
                foreach (var linea in comando.Lineas)
                {
                    if (linea.ProductoId <= 0)
                    {
                        validation.AddError(nameof(linea.ProductoId), "El ID del producto debe ser mayor a 0.");
                    }

                    if (linea.Cantidad <= 0)
                    {
                        validation.AddError(nameof(linea.Cantidad), "La cantidad debe ser mayor a 0.");
                    }
                }
            }

            return validation;
        }
    }
}
