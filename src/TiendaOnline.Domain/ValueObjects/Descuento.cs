using TiendaOnline.SharedKernel;

namespace TiendaOnline.Domain.ValueObjects
{
    /// <summary>
    /// Value Object que representa un descuento monetario no negativo.
    /// </summary>
    public record Descuento
    {
        /// <summary>
        /// Valor numérico del descuento.
        /// </summary>
        public decimal Valor { get; }

        private Descuento(decimal valor)
        {
            Valor = valor;
        }

        /// <summary>
        /// Crea una nueva instancia de descuento con validación.
        /// </summary>
        /// <param name="valor">Valor del descuento.</param>
        /// <returns>Tupla con el descuento creado y el resultado de validación.</returns>
        public static (Descuento? Descuento, ValidationResult Validation) Create(decimal valor)
        {
            var validation = new ValidationResult();

            if (valor < 0)
                validation.AddError("Descuento", "El descuento no puede ser negativo.");

            if (!validation.IsValid)
                return (null, validation);

            return (new Descuento(valor), validation);
        }
    }
}
