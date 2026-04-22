using TiendaOnline.Application.Interfaces;
using TiendaOnline.Contracts;
using TiendaOnline.Domain.Aggregates;
using TiendaOnline.Domain.Repositories;
using TiendaOnline.Domain.Specifications;
using TiendaOnline.Domain.ValueObjects;
using TiendaOnline.SharedKernel;

namespace TiendaOnline.Application.Services
{
    /// <summary>
    /// Servicio de aplicación que gestiona las operaciones relacionadas con pedidos.
    /// </summary>
    public class PedidoApplicationService : IPedidoApplicationService
    {
        private readonly IRepositorioPedido _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProductoRepository _productoRepository;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de pedidos.
        /// </summary>
        /// <param name="pedidoRepository">Repositorio de pedidos.</param>
        /// <param name="clienteRepository">Repositorio de clientes.</param>
        /// <param name="productoRepository">Repositorio de productos.</param>
        public PedidoApplicationService(IRepositorioPedido pedidoRepository, IClienteRepository clienteRepository, IProductoRepository productoRepository)
        {
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
            _productoRepository = productoRepository;
        }

        /// <inheritdoc />
        public async Task<(int? PedidoId, ValidationResult Validation)> CrearPedido(CrearPedidoDTO dto)
        {
            var validation = new ValidationResult();

            var cliente = await _clienteRepository.ObtenerPorIdAsync(dto.ClienteId);
            if (cliente is null)
            {
                validation.AddError("ClienteId", "Cliente no encontrado.");
                return (null, validation);
            }

            Descuento? descuento = null;
            if (dto.Descuento.HasValue)
            {
                var (desc, descVal) = Descuento.Create(dto.Descuento.Value);
                validation.Merge(descVal);
                descuento = desc;
            }

            Direccion? direccionEnvio = null;
            if (dto.DireccionEnvio is not null)
            {
                var (dir, dirVal) = Direccion.Create(dto.DireccionEnvio.Calle, dto.DireccionEnvio.Ciudad, dto.DireccionEnvio.CodigoPostal);
                validation.Merge(dirVal);
                direccionEnvio = dir;
            }
            else
            {
                direccionEnvio = cliente.Direccion;
            }

            if (direccionEnvio is null)
            {
                validation.AddError("DireccionEnvio", "Se requiere una dirección de envío porque el cliente no tiene dirección registrada.");
                return (null, validation);
            }

            if (!validation.IsValid)
                return (null, validation);

            var (pedido, pedidoVal) = Pedido.Create(cliente.Id, direccionEnvio, descuento);
            validation.Merge(pedidoVal);

            if (!validation.IsValid)
                return (null, validation);

            foreach (var lineaDto in dto.Lineas)
            {
                var producto = await _productoRepository.ObtenerPorIdAsync(lineaDto.ProductoId);
                if (producto is null)
                {
                    validation.AddError("ProductoId", $"Producto con Id '{lineaDto.ProductoId}' no encontrado.");
                    continue;
                }

                var (cantidad, cantidadVal) = Cantidad.Create(lineaDto.Cantidad);
                validation.Merge(cantidadVal);

                var (precio, precioVal) = Precio.Create(lineaDto.PrecioUnitario);
                validation.Merge(precioVal);

                if (cantidad is not null && precio is not null)
                {
                    var lineaVal = pedido!.AgregarLinea(producto.Id, producto.Nombre, cantidad, precio);
                    validation.Merge(lineaVal);
                }
            }

            if (!validation.IsValid)
                return (null, validation);

            var confirmarVal = pedido!.Confirmar();
            validation.Merge(confirmarVal);

            if (!validation.IsValid)
                return (null, validation);

            _pedidoRepository.Agregar(pedido);
            await _pedidoRepository.GuardarCambiosAsync();
            return (pedido.Id, validation);
        }

        /// <inheritdoc />
        public async Task<List<Pedido>> ObtenerPedidosPorCliente(int clienteId)
        {
            var especificacion = new PedidoClienteEspecificacion(clienteId);
            return await _pedidoRepository.ObtenerPorEspecificacionAsync(especificacion);
        }

        /// <inheritdoc />
        public async Task<List<Pedido>> ObtenerPedidosPendientesDeEnvio()
        {
            var especificacion = new PedidoPendienteDeEnvioEspecificacion();
            return await _pedidoRepository.ObtenerConLineasPorEspecificacionAsync(especificacion);
        }

        /// <inheritdoc />
        public async Task<List<Pedido>> ObtenerPedidosPendientesDeConfirmacion()
        {
            var especificacion = new PedidoPendienteDeConfirmacionEspecificacion();
            return await _pedidoRepository.ObtenerPorEspecificacionAsync(especificacion);
        }

        /// <inheritdoc />
        public async Task<List<Pedido>> ObtenerPedidosConDescuentoDeCliente(int clienteId)
        {
            var clienteSpec = new PedidoClienteEspecificacion(clienteId);
            var conDescuentoSpec = new PedidoConDescuentoEspecificacion();
            var conLineasSpec = new PedidoConLineasEspecificacion();

            var especificacionCombinada = clienteSpec.Y(conDescuentoSpec).Y(conLineasSpec);

            return await _pedidoRepository.ObtenerConLineasPorEspecificacionAsync(especificacionCombinada);
        }

    }
}
