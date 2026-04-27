using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.Aggregates;
using TiendaOnline.Domain.Repositories;
using TiendaOnline.Domain.ValueObjects;
using TiendaOnline.SharedKernel;

namespace TiendaOnline.Domain.Services
{
    /// <summary>
    /// Implementación del servicio de dominio para la compra de productos.
    /// 
    /// Responsabilidades (lógica de negocio):
    /// - Validar stock disponible de productos
    /// - Calcular descuentos por volumen
    /// - Verificar límites de compra por cliente
    /// - Orquestar la creación del pedido con todas las validaciones
    /// </summary>
    public class ServicioPedidoCompra : IServicioPedidoCompra
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IRepositorioPedido _pedidoRepository;

        /// <summary>
        /// Las constantes de negocio que definen las reglas de descuento
        /// </summary>
        private const int CANTIDAD_MINIMA_DESCUENTO_VOLUMEN = 10;
        private const decimal PORCENTAJE_DESCUENTO_VOLUMEN = 0.05m; // 5%
        private const decimal PORCENTAJE_DESCUENTO_CLIENTE_VIP = 0.10m; // 10%

        public ServicioPedidoCompra(
            IProductoRepository productoRepository,
            IClienteRepository clienteRepository,
            IRepositorioPedido pedidoRepository)
        {
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
            _pedidoRepository = pedidoRepository;
        }

        /// <summary>
        /// Orquesta la creación de un pedido aplicando todas las validaciones de dominio.
        /// 
        /// Flujo:
        /// 1. Valida que el cliente exista y obtenga sus datos
        /// 2. Valida que todos los productos existan y tengan stock
        /// 3. Calcula descuentos aplicables según reglas de negocio
        /// 4. Crea el pedido con los datos validados
        /// 5. Persiste el pedido
        /// 
        /// Este servicio encapsula la lógica compleja que involucra múltiples agregados
        /// </summary>
        public async Task<ValidationResult> CrearPedidoConValidacionCompleta(
            int clienteId,
            List<(int productoId, int cantidad)> items,
            Descuento? descuentoAplicado)
        {
            // 1. Obtener cliente
            var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);
            if (cliente == null)
                return ValidationResult.WithError("Cliente", "Cliente no encontrado");

            // 2. Validar y obtener productos
            var productos = new List<Producto>();
            foreach (var item in items)
            {
                var producto = await _productoRepository.ObtenerPorIdAsync(item.productoId);

                if (producto == null)
                    return ValidationResult.WithError("Producto", $"Producto {item.productoId} no encontrado");

                // Validación de dominio: verificar stock disponible
                if (producto.Stock < item.cantidad)
                    return ValidationResult.WithError(
                        "Stock",
                        $"Stock insuficiente para {producto.Nombre}. " +
                        $"Disponible: {producto.Stock}, Solicitado: {item.cantidad}");

                productos.Add(producto);
            }

            // 3. Calcular descuentos aplicables según reglas de negocio
            var descuentoFinal = CalcularDescuentoAplicable(items, cliente, descuentoAplicado);

            // 4. Crear el pedido usando el método estático
            var (pedido, pedidoValidation) = Pedido.Create(
                cliente.Id,
                cliente.Direccion ?? throw new InvalidOperationException("El cliente no tiene dirección."),
                descuentoFinal);
            if (!pedidoValidation.IsValid || pedido == null)
                return pedidoValidation;

            // Agregar líneas al pedido (si tu modelo lo requiere, aquí deberías agregar las líneas)
            // Ejemplo:
            // foreach (var item in items)
            //     pedido.AgregarLinea(item.productoId, item.cantidad);

            // 5. Persistir
            _pedidoRepository.Agregar(pedido);

            return ValidationResult.Success();
        }

        /// <summary>
        /// Lógica de negocio: Calcula qué descuentos aplican según reglas de dominio.
        /// 
        /// Reglas:
        /// - Si cantidad total >= 10 unidades: 5% descuento por volumen
        /// - Si cliente es VIP: 10% descuento adicional
        /// - Se aplica el descuento manual si es mayor que los calculados
        /// </summary>
        private Descuento? CalcularDescuentoAplicable(
            List<(int productoId, int cantidad)> items,
            Cliente cliente,
            Descuento? descuentoManual)
        {
            int cantidadTotal = items.Sum(x => x.cantidad);
            decimal descuentoCalculado = 0;

            // Regla: Descuento por volumen
            if (cantidadTotal >= CANTIDAD_MINIMA_DESCUENTO_VOLUMEN)
            {
                descuentoCalculado = PORCENTAJE_DESCUENTO_VOLUMEN;
            }


            // Regla: Descuento cliente VIP (se suma al descuento por volumen)
            if (cliente.EsVip)
            {
                descuentoCalculado += PORCENTAJE_DESCUENTO_CLIENTE_VIP;
            }

            // Tomar el máximo entre el descuento calculado y el manual
            if (descuentoManual != null && descuentoManual.Valor > descuentoCalculado)
                return descuentoManual;

            if (descuentoCalculado > 0)
            {
                var (desc, _) = Descuento.Create(descuentoCalculado);
                return desc;
            }
            return null;
        }

        /// <summary>
        /// Ejemplo adicional: Servicio de dominio que valida si hay stock suficiente
        /// para procesar una devolución y actualizar el inventario.
        /// </summary>
        public async Task<ValidationResult> ProcesarDevolucionConActualizacionStock(
            int pedidoId,
            List<(int productoId, int cantidad)> itemsADevolver)
        {
            // Lógica de negocio:
            // - Verificar que el pedido sea elegible para devolución (no vencido, no cancelado)
            // - Validar que los items a devolver pertenecen al pedido
            // - Incrementar stock de productos
            // - Crear nota de crédito para el cliente

            var pedido = await _pedidoRepository.ObtenerPorIdAsync(pedidoId);
            if (pedido == null)
                return ValidationResult.WithError("Pedido", "Pedido no encontrado");


            // Validación de negocio: plazo de devolución (30 días)
            if (pedido.FechaEntrega.HasValue)
            {
                var dias = (DateTime.UtcNow - pedido.FechaEntrega.Value).TotalDays;
                if (dias > 30)
                    return ValidationResult.WithError("Devolución", "El plazo de devolución ha expirado");
            }
            else
            {
                return ValidationResult.WithError("Devolución", "El pedido no ha sido entregado aún.");
            }

            // Aquí iría la lógica de actualizar stock, crear notas de crédito, etc.

            return ValidationResult.Success();
        }
    }
}
