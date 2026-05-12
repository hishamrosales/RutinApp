using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    /// <summary>
    /// Modelo que representa la relación entre una rutina
    /// y los ejercicios que la componen.
    /// 
    /// Esta entidad funciona como tabla intermedia
    /// entre <see cref="RutinaModel"/> y
    /// <see cref="EjercicioModel"/>,
    /// permitiendo almacenar información adicional
    /// específica de cada ejercicio dentro de una rutina.
    /// 
    /// La información registrada incluye:
    /// - Número de series.
    /// - Número de repeticiones.
    /// </summary>
    public class RutinaDetalleModel
    {
        /// <summary>
        /// Identificador de la rutina asociada.
        /// </summary>
        public int RutinaId { get; set; }

        /// <summary>
        /// Identificador del ejercicio asociado.
        /// </summary>
        public int EjercicioId { get; set; }

        /// <summary>
        /// Cantidad de series configuradas
        /// para el ejercicio dentro de la rutina.
        /// </summary>
        public int Series { get; set; }

        /// <summary>
        /// Cantidad de repeticiones configuradas
        /// para el ejercicio dentro de la rutina.
        /// </summary>
        public int Repeticiones { get; set; }

        /// <summary>
        /// Propiedad de navegación hacia la rutina
        /// asociada al detalle.
        /// </summary>
        public RutinaModel? Rutina { get; set; }

        /// <summary>
        /// Propiedad de navegación hacia el ejercicio
        /// asociado al detalle.
        /// </summary>
        public EjercicioModel? Ejercicio { get; set; }
    }
}