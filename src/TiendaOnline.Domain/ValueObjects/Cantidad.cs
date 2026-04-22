using TiendaOnline.SharedKernel;

namespace TiendaOnline.Domain.ValueObjects
{
    /// <summary>
    /// Value Object que representa una cantidad positiva de unidades.
    /// </summary>
    public record Cantidad
    {
        /// <summary>
        /// Valor numérico de la cantidad.
        /// </summary>
        public int Value { get; }

        private Cantidad(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Crea una nueva instancia de cantidad con validación.
        /// </summary>
        /// <param name="value">Valor de la cantidad.</param>
        /// <returns>Tupla con la cantidad creada y el resultado de validación.</returns>
        public static (Cantidad? Cantidad, ValidationResult Validation) Create(int value)
        {
            var validation = new ValidationResult();

            if (value <= 0)
                validation.AddError("Cantidad", "La cantidad debe ser mayor a cero.");

            if (!validation.IsValid)
                return (null, validation);

            return (new Cantidad(value), validation);
        }
    }
}
