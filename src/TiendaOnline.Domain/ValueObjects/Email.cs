using System.Text.RegularExpressions;
using TiendaOnline.SharedKernel;

namespace TiendaOnline.Domain.ValueObjects
{
    /// <summary>
    /// Value Object que representa una dirección de correo electrónico válida.
    /// </summary>
    public record Email
    {
        /// <summary>
        /// Valor del correo electrónico.
        /// </summary>
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Crea una nueva instancia de email con validación de formato.
        /// </summary>
        /// <param name="value">Dirección de correo electrónico.</param>
        /// <returns>Tupla con el email creado y el resultado de validación.</returns>
        public static (Email? Email, ValidationResult Validation) Create(string value)
        {
            var validation = new ValidationResult();

            if (string.IsNullOrWhiteSpace(value))
                validation.AddError("Email", "El email no puede estar vacío.");
            else if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                validation.AddError("Email", "El formato del email no es válido.");

            if (!validation.IsValid)
                return (null, validation);

            return (new Email(value), validation);
        }
    }
}
