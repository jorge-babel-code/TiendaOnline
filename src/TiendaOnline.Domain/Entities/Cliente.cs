using TiendaOnline.Domain.ValueObjects;
using TiendaOnline.SharedKernel;

namespace TiendaOnline.Domain.Entities
{
    /// <summary>
    /// Representa un cliente del sistema.
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// Indica si el cliente es VIP.
        /// </summary>
        public bool EsVip { get; private set; }
        /// <summary>
        /// Identificador único del cliente.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Nombre completo del cliente.
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Correo electrónico del cliente.
        /// </summary>
        public Email Email { get; private set; }

        /// <summary>
        /// Dirección del cliente, puede ser nula.
        /// </summary>
        public Direccion? Direccion { get; private set; }

        private Cliente(string nombre, Email email, Direccion? direccion)
        {
            Nombre = nombre;
            Email = email;
            Direccion = direccion;
            EsVip = false;
        }

        private Cliente(string nombre, Email email, Direccion? direccion, bool esVip)
        {
            Nombre = nombre;
            Email = email;
            Direccion = direccion;
            EsVip = esVip;
        }

        /// <summary>
        /// Crea una nueva instancia de cliente con validación.
        /// </summary>
        /// <param name="nombre">Nombre del cliente.</param>
        /// <param name="email">Correo electrónico del cliente.</param>
        /// <param name="direccion">Dirección opcional del cliente.</param>
        /// <returns>Tupla con el cliente creado y el resultado de validación.</returns>
        public static (Cliente? Cliente, ValidationResult Validation) Create(string nombre, Email email, Direccion? direccion = null)
        {
            var validation = new ValidationResult();

            if (string.IsNullOrWhiteSpace(nombre))
                validation.AddError("Nombre", "El nombre no puede estar vacío.");

            if (!validation.IsValid)
                return (null, validation);

            return (new Cliente(nombre, email, direccion), validation);
        }

        /// <summary>
        /// Crea una nueva instancia de cliente con validación y estado VIP.
        /// </summary>
        /// <param name="nombre">Nombre del cliente.</param>
        /// <param name="email">Correo electrónico del cliente.</param>
        /// <param name="direccion">Dirección opcional del cliente.</param>
        /// <param name="esVip">Indica si el cliente es VIP.</param>
        /// <returns>Tupla con el cliente creado y el resultado de validación.</returns>
        public static (Cliente? Cliente, ValidationResult Validation) Create(string nombre, Email email, Direccion? direccion, bool esVip)
        {
            var validation = new ValidationResult();

            if (string.IsNullOrWhiteSpace(nombre))
                validation.AddError("Nombre", "El nombre no puede estar vacío.");

            if (!validation.IsValid)
                return (null, validation);

            return (new Cliente(nombre, email, direccion, esVip), validation);
        }

        /// <summary>
        /// Cambia el correo electrónico del cliente.
        /// </summary>
        /// <param name="nuevoEmail">Nuevo correo electrónico.</param>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult CambiarEmail(Email nuevoEmail)
        {
            var validation = new ValidationResult();

            if (nuevoEmail is null)
                validation.AddError("Email", "El email es obligatorio.");

            if (!validation.IsValid)
                return validation;

            Email = nuevoEmail!;
            return validation;
        }

        /// <summary>
        /// Cambia la dirección del cliente.
        /// </summary>
        /// <param name="nuevaDireccion">Nueva dirección, puede ser nula.</param>
        /// <returns>Resultado de la validación.</returns>
        public ValidationResult CambiarDireccion(Direccion? nuevaDireccion)
        {
            Direccion = nuevaDireccion;
            return ValidationResult.Success();
        }
    }
}
