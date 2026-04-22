namespace TiendaOnline.SharedKernel
{
    /// <summary>
    /// Representa el resultado de una validación con posibles errores.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Indica si la validación fue exitosa (sin errores).
        /// </summary>
        public bool IsValid => Errors.Count == 0;

        /// <summary>
        /// Lista de errores de validación encontrados.
        /// </summary>
        public List<ValidationError> Errors { get; } = new();

        /// <summary>
        /// Crea un resultado de validación exitoso sin errores.
        /// </summary>
        /// <returns>Un nuevo <see cref="ValidationResult"/> sin errores.</returns>
        public static ValidationResult Success() => new();

        /// <summary>
        /// Crea un resultado de validación con un único error.
        /// </summary>
        /// <param name="field">Campo que falló la validación.</param>
        /// <param name="message">Mensaje descriptivo del error.</param>
        /// <returns>Un nuevo <see cref="ValidationResult"/> con el error especificado.</returns>
        public static ValidationResult WithError(string field, string message)
        {
            var result = new ValidationResult();
            result.Errors.Add(new ValidationError(field, message));
            return result;
        }

        /// <summary>
        /// Agrega un error de validación a la lista de errores.
        /// </summary>
        /// <param name="field">Campo que falló la validación.</param>
        /// <param name="message">Mensaje descriptivo del error.</param>
        public void AddError(string field, string message)
        {
            Errors.Add(new ValidationError(field, message));
        }

        /// <summary>
        /// Combina los errores de otro resultado de validación con este.
        /// </summary>
        /// <param name="other">Otro resultado de validación cuyos errores se agregarán.</param>
        public void Merge(ValidationResult other)
        {
            Errors.AddRange(other.Errors);
        }
    }

    /// <summary>
    /// Representa un error de validación con el campo y mensaje asociados.
    /// </summary>
    /// <param name="Field">Nombre del campo que falló la validación.</param>
    /// <param name="Message">Mensaje descriptivo del error.</param>
    public record ValidationError(string Field, string Message);
}
