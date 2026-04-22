using TiendaOnline.SharedKernel;

namespace TiendaOnline.Domain.ValueObjects
{
    /// <summary>
    /// Value Object que representa una dirección postal.
    /// </summary>
    public record Direccion
    {
        /// <summary>
        /// Nombre de la calle y número.
        /// </summary>
        public string Calle { get; }

        /// <summary>
        /// Nombre de la ciudad.
        /// </summary>
        public string Ciudad { get; }

        /// <summary>
        /// Código postal.
        /// </summary>
        public string CodigoPostal { get; }

        private Direccion(string calle, string ciudad, string codigoPostal)
        {
            Calle = calle;
            Ciudad = ciudad;
            CodigoPostal = codigoPostal;
        }

        /// <summary>
        /// Crea una nueva instancia de dirección con validación.
        /// </summary>
        /// <param name="calle">Nombre de la calle.</param>
        /// <param name="ciudad">Nombre de la ciudad.</param>
        /// <param name="codigoPostal">Código postal.</param>
        /// <returns>Tupla con la dirección creada y el resultado de validación.</returns>
        public static (Direccion? Direccion, ValidationResult Validation) Create(string calle, string ciudad, string codigoPostal)
        {
            var validation = new ValidationResult();

            if (string.IsNullOrWhiteSpace(calle))
                validation.AddError("Calle", "La calle no puede estar vacía.");

            if (string.IsNullOrWhiteSpace(ciudad))
                validation.AddError("Ciudad", "La ciudad no puede estar vacía.");

            if (string.IsNullOrWhiteSpace(codigoPostal))
                validation.AddError("CodigoPostal", "El código postal no puede estar vacío.");
            else if (codigoPostal.Length < 3 || codigoPostal.Length > 10)
                validation.AddError("CodigoPostal", "El código postal debe tener entre 3 y 10 caracteres.");

            if (!validation.IsValid)
                return (null, validation);

            return (new Direccion(calle, ciudad, codigoPostal), validation);
        }
    }
}
