/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.5
/// </summary>
namespace RutinApp.Models
{
    /// <summary>
    /// Modelo utilizado para representar un elemento
    /// del historial de rutinas realizadas por el usuario.
    /// 
    /// Cada instancia almacena información resumida
    /// sobre una rutina ejecutada, incluyendo:
    /// - Nombre de la rutina.
    /// - Fecha de realización.
    /// - Resumen o descripción de la sesión.
    /// </summary>
    public class HistorialItem
    {
        /// <summary>
        /// Obtiene o establece el nombre de la rutina
        /// registrada en el historial.
        /// </summary>
        public string NombreRutina { get; set; } = string.Empty;

        /// <summary>
        /// Obtiene o establece la fecha en la que
        /// se realizó la rutina.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Obtiene o establece un resumen descriptivo
        /// de la rutina realizada.
        /// 
        /// Este texto puede incluir información como:
        /// - Duración.
        /// - Ejercicios realizados.
        /// - Observaciones generales.
        /// </summary>
        public string Resumen { get; set; } = string.Empty;
    }
}