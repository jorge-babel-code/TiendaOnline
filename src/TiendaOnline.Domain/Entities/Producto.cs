using TiendaOnline.SharedKernel;
using TiendaOnline.Domain.ValueObjects;

namespace TiendaOnline.Domain.Entities
{
    /// <summary>
    /// Representa un producto disponible en la tienda.
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Nombre del producto.
        /// </summary>
        public string Nombre { get; private set; }

        /// <summary>
        /// Precio base del producto antes de aplicar descuentos.
        /// </summary>
        public Precio PrecioBase { get; private set; }

        /// <summary>
        /// Porcentaje de descuento aplicado al producto (0-100).
        /// </summary>
        public decimal PorcentajeDescuento { get; private set; }

        /// <summary>
        /// Cantidad de unidades disponibles en inventario.
        /// </summary>
        public int Stock { get; private set; }

        /// <summary>
        /// Indica si el producto está activo para la venta.
        /// </summary>
        public bool EstaActivo { get; private set; }

        /// <summary>
        /// Precio final calculado después de aplicar el descuento.
        /// </summary>
        public decimal PrecioConDescuento => PrecioBase.Value * (1 - PorcentajeDescuento / 100);

        private Producto(string nombre, Precio precioBase, int stockInicial, bool estaActivo)
        {
            Nombre = nombre;
            PrecioBase = precioBase;
            Stock = stockInicial;
            PorcentajeDescuento = 0;
            EstaActivo = estaActivo;
        }

        /// <summary>
        /// Crea una nueva instancia de producto con validación.
        /// </summary>
        /// <param name="nombre">Nombre del producto.</param>
        /// <param name="precioBase">Precio base del producto.</param>
        /// <param name="stockInicial">Stock inicial disponible.</param>
        /// <param name="estaActivo">Indica si el producto está activo.</param>
        /// <returns>Tupla con el producto creado y el resultado de validación.</returns>
        public static (Producto? Producto, ValidationResult Validation) Create(
            string nombre, 
            decimal precioBase, 
            int stockInicial = 0, 
            bool estaActivo = true)
        {
            var validation = new ValidationResult();

            if (string.IsNullOrWhiteSpace(nombre))
                validation.AddError("Nombre", "El nombre del producto no puede estar vacío.");

            var (precio, precioValidation) = Precio.Create(precioBase);
            validation.Merge(precioValidation);

            if (stockInicial < 0)
                validation.AddError("Stock", "El stock inicial no puede ser negativo.");

            if (!validation.IsValid)
                return (null, validation);

            return (new Producto(nombre, precio!, stockInicial, estaActivo), validation);
        }

        /// <summary>
        /// Reduce el stock del producto en la cantidad especificada.
        /// </summary>
        /// <param name="cantidad">Cantidad a reducir del stock.</param>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult ReducirStock(int cantidad)
        {
            var validation = new ValidationResult();

            if (cantidad <= 0)
                validation.AddError("Cantidad", "La cantidad a reducir debe ser mayor que cero.");

            if (cantidad > Stock)
                validation.AddError("Stock", $"No hay suficiente stock. Stock actual: {Stock}, cantidad solicitada: {cantidad}.");

            if (!validation.IsValid)
                return validation;

            Stock -= cantidad;
            return ValidationResult.Success();
        }

        /// <summary>
        /// Repone el stock del producto añadiendo la cantidad especificada.
        /// </summary>
        /// <param name="cantidad">Cantidad a añadir al stock.</param>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult ReponerStock(int cantidad)
        {
            var validation = new ValidationResult();

            if (cantidad <= 0)
                validation.AddError("Cantidad", "La cantidad a reponer debe ser mayor que cero.");

            if (!validation.IsValid)
                return validation;

            Stock += cantidad;
            return ValidationResult.Success();
        }

        /// <summary>
        /// Aplica un porcentaje de descuento al producto.
        /// </summary>
        /// <param name="porcentaje">Porcentaje de descuento (0-100).</param>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult AplicarDescuento(decimal porcentaje)
        {
            var validation = new ValidationResult();

            if (porcentaje < 0 || porcentaje > 100)
                validation.AddError("PorcentajeDescuento", "El porcentaje de descuento debe estar entre 0 y 100.");

            if (!validation.IsValid)
                return validation;

            PorcentajeDescuento = porcentaje;
            return ValidationResult.Success();
        }

        /// <summary>
        /// Elimina cualquier descuento aplicado al producto.
        /// </summary>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult QuitarDescuento()
        {
            PorcentajeDescuento = 0;
            return ValidationResult.Success();
        }

        /// <summary>
        /// Desactiva el producto para que no esté disponible para la venta.
        /// </summary>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult Desactivar()
        {
            EstaActivo = false;
            return ValidationResult.Success();
        }
    }
}
