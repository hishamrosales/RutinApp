using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Models
{
    /// <summary>
    /// Modelo que representa el detalle de un ejercicio
    /// realizado durante una sesión de entrenamiento.
    /// 
    /// Esta entidad almacena información específica
    /// de desempeño registrada en el historial,
    /// incluyendo:
    /// - Ejercicio realizado.
    /// - Peso utilizado.
    /// - Repeticiones logradas.
    /// - Relación con el historial de entrenamiento.
    /// </summary>
    public class DetalleLogModel
    {
        /// <summary>
        /// Identificador único del detalle de entrenamiento.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador del historial de entrenamiento
        /// al que pertenece el registro.
        /// </summary>
        public int HistorialId { get; set; }

        /// <summary>
        /// Identificador del ejercicio realizado.
        /// </summary>
        public int EjercicioId { get; set; }

        /// <summary>
        /// Peso utilizado durante la ejecución
        /// del ejercicio.
        /// </summary>
        public int PesoUtilizado { get; set; }

        /// <summary>
        /// Cantidad de repeticiones logradas
        /// durante la serie registrada.
        /// </summary>
        public int RepsLogradas { get; set; }

        /// <summary>
        /// Propiedad de navegación hacia el historial
        /// de entrenamiento asociado.
        /// </summary>
        public HistorialEntrenamientoModel? Historial { get; set; }

        /// <summary>
        /// Propiedad de navegación hacia el ejercicio
        /// asociado al registro.
        /// </summary>
        public EjercicioModel? Ejercicio { get; set; }
    }
}