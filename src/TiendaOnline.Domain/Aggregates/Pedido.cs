using TiendaOnline.Domain.Entities;
using TiendaOnline.Domain.ValueObjects;
using TiendaOnline.SharedKernel;

namespace TiendaOnline.Domain.Aggregates
{
    /// <summary>
    /// Agregado raíz que representa un pedido en el sistema.
    /// </summary>
    public class Pedido
    {
        /// <summary>
        /// Identificador único del pedido.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Identificador del cliente que realizó el pedido.
        /// </summary>
        public int ClienteId { get; private set; }

        private readonly List<LineaPedido> _lineas = new();

        /// <summary>
        /// Líneas de productos incluidas en el pedido.
        /// </summary>
        public IReadOnlyList<LineaPedido> Lineas => _lineas.AsReadOnly();

        /// <summary>
        /// Descuento aplicado al pedido, si existe.
        /// </summary>
        public Descuento? Descuento { get; private set; }

        /// <summary>
        /// Dirección de envío del pedido.
        /// </summary>
        public Direccion DireccionEnvio { get; private set; }

        /// <summary>
        /// Estado actual del pedido.
        /// </summary>
        public EstadoPedido Estado { get; private set; }

        /// <summary>
        /// Fecha y hora de creación del pedido.
        /// </summary>
        public DateTime FechaCreacion { get; private set; }

        /// <summary>
        /// Fecha y hora de confirmación del pedido.
        /// </summary>
        public DateTime? FechaConfirmacion { get; private set; }

        /// <summary>
        /// Fecha y hora de envío del pedido.
        /// </summary>
        public DateTime? FechaEnvio { get; private set; }

        /// <summary>
        /// Fecha y hora de entrega del pedido.
        /// </summary>
        public DateTime? FechaEntrega { get; private set; }

        /// <summary>
        /// Fecha y hora de cancelación del pedido.
        /// </summary>
        public DateTime? FechaCancelacion { get; private set; }

        /// <summary>
        /// Total del pedido calculado restando el descuento del subtotal de líneas.
        /// </summary>
        public decimal Total => (_lineas.Sum(l => l.Subtotal)) - (Descuento?.Valor ?? 0);

        private Pedido(int clienteId, Direccion direccionEnvio, Descuento? descuento)
        {
            ClienteId = clienteId;
            DireccionEnvio = direccionEnvio;
            Descuento = descuento;
            Estado = EstadoPedido.Borrador;
            FechaCreacion = DateTime.UtcNow;
        }

        /// <summary>
        /// Crea una nueva instancia de pedido con validación.
        /// </summary>
        /// <param name="clienteId">Identificador del cliente.</param>
        /// <param name="direccionEnvio">Dirección de envío del pedido.</param>
        /// <param name="descuento">Descuento opcional a aplicar.</param>
        /// <returns>Tupla con el pedido creado y el resultado de validación.</returns>
        public static (Pedido? Pedido, ValidationResult Validation) Create(int clienteId, Direccion direccionEnvio, Descuento? descuento = null)
        {
            var validation = new ValidationResult();

            if (clienteId <= 0)
                validation.AddError("ClienteId", "El cliente es obligatorio.");

            if (direccionEnvio is null)
                validation.AddError("DireccionEnvio", "La dirección de envío es obligatoria.");

            if (!validation.IsValid)
                return (null, validation);

            return (new Pedido(clienteId, direccionEnvio!, descuento), validation);
        }

        /// <summary>
        /// Agrega una línea de producto al pedido.
        /// </summary>
        /// <param name="productoId">Identificador del producto.</param>
        /// <param name="productoNombre">Nombre del producto.</param>
        /// <param name="cantidad">Cantidad de unidades.</param>
        /// <param name="precioUnitario">Precio por unidad.</param>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult AgregarLinea(int productoId, string productoNombre, Cantidad cantidad, Precio precioUnitario)
        {
            var validation = new ValidationResult();

            if (Estado != EstadoPedido.Borrador)
            {
                validation.AddError("Estado", "Solo se pueden agregar líneas a un pedido en estado Borrador.");
                return validation;
            }

            if (productoId <= 0)
                validation.AddError("ProductoId", "El producto es obligatorio.");

            if (string.IsNullOrWhiteSpace(productoNombre))
                validation.AddError("ProductoNombre", "El nombre del producto es obligatorio.");

            if (cantidad is null)
                validation.AddError("Cantidad", "La cantidad es obligatoria.");

            if (precioUnitario is null)
                validation.AddError("PrecioUnitario", "El precio unitario es obligatorio.");

            if (!validation.IsValid)
                return validation;

            _lineas.Add(new LineaPedido(productoId, productoNombre, cantidad!, precioUnitario!));
            return validation;
        }

        /// <summary>
        /// Elimina una línea del pedido por su identificador.
        /// </summary>
        /// <param name="lineaId">Identificador de la línea a eliminar.</param>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult EliminarLinea(int lineaId)
        {
            var validation = new ValidationResult();

            if (Estado != EstadoPedido.Borrador)
            {
                validation.AddError("Estado", "Solo se pueden eliminar líneas de un pedido en estado Borrador.");
                return validation;
            }

            var linea = _lineas.FirstOrDefault(l => l.Id == lineaId);

            if (linea is null)
            {
                validation.AddError("LineaId", "La línea de pedido no existe.");
                return validation;
            }

            _lineas.Remove(linea);
            return validation;
        }

        /// <summary>
        /// Cambia la dirección de envío del pedido.
        /// </summary>
        /// <param name="nuevaDireccion">Nueva dirección de envío.</param>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult CambiarDireccionEnvio(Direccion nuevaDireccion)
        {
            var validation = new ValidationResult();

            if (Estado != EstadoPedido.Borrador)
            {
                validation.AddError("Estado", "Solo se puede cambiar la dirección de envío en estado Borrador.");
                return validation;
            }

            if (nuevaDireccion is null)
                validation.AddError("DireccionEnvio", "La dirección de envío es obligatoria.");

            if (!validation.IsValid)
                return validation;

            DireccionEnvio = nuevaDireccion!;
            return validation;
        }

        /// <summary>
        /// Confirma el pedido, cambiando su estado de Borrador a Confirmado.
        /// </summary>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult Confirmar()
        {
            var validation = new ValidationResult();

            if (Estado != EstadoPedido.Borrador)
            {
                validation.AddError("Estado", "Solo se puede confirmar un pedido en estado Borrador.");
                return validation;
            }

            if (_lineas.Count == 0)
            {
                validation.AddError("Lineas", "El pedido debe tener al menos una línea para poder confirmarse.");
                return validation;
            }

            Estado = EstadoPedido.Confirmado;
            FechaConfirmacion = DateTime.UtcNow;
            return validation;
        }

        /// <summary>
        /// Marca el pedido como enviado.
        /// </summary>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult Enviar()
        {
            var validation = new ValidationResult();

            if (Estado != EstadoPedido.Confirmado)
            {
                validation.AddError("Estado", "Solo se puede enviar un pedido en estado Confirmado.");
                return validation;
            }

            Estado = EstadoPedido.Enviado;
            FechaEnvio = DateTime.UtcNow;
            return validation;
        }

        /// <summary>
        /// Marca el pedido como entregado.
        /// </summary>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult Entregar()
        {
            var validation = new ValidationResult();

            if (Estado != EstadoPedido.Enviado)
            {
                validation.AddError("Estado", "Solo se puede entregar un pedido en estado Enviado.");
                return validation;
            }

            Estado = EstadoPedido.Entregado;
            FechaEntrega = DateTime.UtcNow;
            return validation;
        }

        /// <summary>
        /// Cancela el pedido si aún no ha sido entregado o cancelado.
        /// </summary>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult Cancelar()
        {
            var validation = new ValidationResult();

            if (Estado == EstadoPedido.Entregado || Estado == EstadoPedido.Cancelado)
            {
                validation.AddError("Estado", "No se puede cancelar un pedido ya entregado o cancelado.");
                return validation;
            }

            Estado = EstadoPedido.Cancelado;
            FechaCancelacion = DateTime.UtcNow;
            return validation;
        }
    }
}
