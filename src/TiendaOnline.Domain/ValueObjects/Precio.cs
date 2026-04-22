using TiendaOnline.SharedKernel;

namespace TiendaOnline.Domain.ValueObjects
{
    /// <summary>
    /// Value Object que representa un precio monetario no negativo.
    /// </summary>
    public record Precio
    {
        /// <summary>
        /// Valor numérico del precio.
        /// </summary>
        public decimal Value { get; }

        private Precio(decimal value)
        {
            Value = value;
        }

        /// <summary>
        /// Crea una nueva instancia de precio con validación.
        /// </summary>
        /// <param name="value">Valor del precio.</param>
        /// <returns>Tupla con el precio creado y el resultado de validación.</returns>
        public static (Precio? Precio, ValidationResult Validation) Create(decimal value)
        {
            var validation = new ValidationResult();

            if (value < 0)
                validation.AddError("Precio", "El precio no puede ser negativo.");

            if (!validation.IsValid)
                return (null, validation);

            return (new Precio(value), validation);
        }
    }
}
