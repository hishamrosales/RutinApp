using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    /// <summary>
    /// Modelo que representa un usuario dentro de la aplicación.
    /// 
    /// Esta entidad almacena la información principal
    /// relacionada con los usuarios registrados,
    /// incluyendo:
    /// - Nombre.
    /// - Correo electrónico.
    /// - Contraseña.
    /// 
    /// También mantiene la relación con:
    /// - Rutinas creadas.
    /// - Historiales de entrenamiento.
    /// </summary>
    public class UsuarioModel
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del usuario.
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el correo electrónico
        /// asociado al usuario.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la contraseña del usuario.
        /// 
        /// Nota:
        /// En una aplicación real, las contraseñas
        /// deberían almacenarse utilizando mecanismos
        /// de cifrado o hashing por seguridad.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Colección de rutinas creadas
        /// por el usuario.
        /// </summary>
        public ICollection<RutinaModel> Rutinas { get; set; } = [];

        /// <summary>
        /// Colección de historiales de entrenamiento
        /// asociados al usuario.
        /// </summary>
        public ICollection<HistorialEntrenamientoModel> Historiales { get; set; } = [];
    }
}