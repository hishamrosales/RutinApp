using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    /// <summary>
    /// Modelo que representa una rutina de entrenamiento.
    /// 
    /// Esta entidad almacena la información principal
    /// de una rutina creada por un usuario, incluyendo:
    /// - Nombre de la rutina.
    /// - Fecha de creación.
    /// - Usuario propietario.
    /// 
    /// También mantiene la relación con los ejercicios
    /// que forman parte de la rutina.
    /// </summary>
    public class RutinaModel
    {
        /// <summary>
        /// Identificador único de la rutina.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador del usuario propietario
        /// de la rutina.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre de la rutina.
        /// 
        /// Ejemplo:
        /// "Rutina de pecho y tríceps"
        /// </summary>
        public string NombreRutina { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora en que la rutina fue creada.
        /// 
        /// El valor predeterminado corresponde
        /// a la fecha y hora actual.
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        /// <summary>
        /// Propiedad de navegación hacia el usuario
        /// asociado a la rutina.
        /// </summary>
        public UsuarioModel? Usuario { get; set; }

        /// <summary>
        /// Colección que contiene los ejercicios
        /// asociados a la rutina.
        /// 
        /// Esta relación se administra mediante
        /// <see cref="RutinaDetalleModel"/>.
        /// </summary>
        public ICollection<RutinaDetalleModel> RutinaEjercicios { get; set; } = [];
    }
}