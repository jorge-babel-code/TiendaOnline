namespace TiendaOnline.Contracts
{
    /// <summary>
    /// DTO que representa una dirección postal.
    /// </summary>
    public class DireccionDTO
    {
        /// <summary>
        /// Nombre de la calle y número.
        /// </summary>
        public string Calle { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la ciudad.
        /// </summary>
        public string Ciudad { get; set; } = string.Empty;

        /// <summary>
        /// Código postal.
        /// </summary>
        public string CodigoPostal { get; set; } = string.Empty;
    }
}
