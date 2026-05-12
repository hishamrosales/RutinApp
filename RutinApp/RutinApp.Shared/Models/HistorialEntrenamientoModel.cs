using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    /// <summary>
    /// Modelo que representa el historial
    /// de una sesión de entrenamiento realizada por el usuario.
    /// 
    /// Esta entidad almacena información general
    /// sobre una sesión completada, incluyendo:
    /// - Usuario asociado.
    /// - Fecha de realización.
    /// - Notas o nombre de la rutina.
    /// 
    /// También contiene la relación con los detalles
    /// de ejercicios ejecutados durante la sesión.
    /// </summary>
    public class HistorialEntrenamientoModel
    {
        /// <summary>
        /// Identificador único del historial de entrenamiento.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador del usuario asociado
        /// al historial de entrenamiento.
        /// </summary>
        public int UsuarioId { get; set; }

        /// <summary>
        /// Fecha y hora en que se registró
        /// la sesión de entrenamiento.
        /// 
        /// El valor predeterminado corresponde
        /// a la fecha y hora actual.
        /// </summary>
        public DateTime Fecha { get; set; } = DateTime.Now;

        /// <summary>
        /// Obtiene o establece notas relacionadas
        /// con la sesión de entrenamiento.
        /// 
        /// Generalmente se utiliza para almacenar
        /// el nombre de la rutina realizada
        /// o comentarios adicionales.
        /// </summary>
        public string Notas { get; set; } = string.Empty;

        /// <summary>
        /// Propiedad de navegación hacia el usuario
        /// asociado al historial.
        /// </summary>
        public UsuarioModel? Usuario { get; set; }

        /// <summary>
        /// Colección de detalles registrados
        /// durante la sesión de entrenamiento.
        /// 
        /// Contiene información específica
        /// de ejercicios realizados.
        /// </summary>
        public ICollection<DetalleLogModel> DetalleLogs { get; set; } = [];
    }
}