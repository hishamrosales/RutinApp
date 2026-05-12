using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    /// <summary>
    /// Modelo que representa un ejercicio dentro de la aplicación.
    /// 
    /// Esta entidad almacena la información principal
    /// de cada ejercicio disponible, incluyendo:
    /// - Nombre.
    /// - Grupo muscular.
    /// - Descripción.
    /// 
    /// También contiene relaciones con:
    /// - Rutinas de entrenamiento.
    /// - Registros de historial de ejercicios realizados.
    /// </summary>
    public class EjercicioModel
    {
        /// <summary>
        /// Identificador único del ejercicio.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el nombre del ejercicio.
        /// 
        /// Ejemplo:
        /// "Press de banca"
        /// </summary>
        public string Nombre { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece el grupo muscular
        /// trabajado por el ejercicio.
        /// 
        /// Ejemplo:
        /// "Pecho"
        /// </summary>
        public string GrupoMuscular { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece una descripción
        /// detallada del ejercicio.
        /// 
        /// Puede incluir:
        /// - Técnica.
        /// - Recomendaciones.
        /// - Observaciones.
        /// </summary>
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Colección de relaciones entre rutinas
        /// y ejercicios.
        /// 
        /// Permite identificar en qué rutinas
        /// se encuentra asociado el ejercicio.
        /// </summary>
        public ICollection<RutinaDetalleModel> RutinaEjercicios { get; set; } = [];

        /// <summary>
        /// Colección de registros históricos
        /// relacionados con el ejercicio.
        /// 
        /// Almacena información de desempeño
        /// durante sesiones de entrenamiento.
        /// </summary>
        public ICollection<DetalleLogModel> DetalleLogs { get; set; } = [];
    }
}