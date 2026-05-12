using CommunityToolkit.Mvvm.ComponentModel;

namespace RutinApp.Shared.Models
{
    /// <summary>
    /// Modelo utilizado para representar el registro
    /// de una serie realizada durante una sesión
    /// de entrenamiento.
    /// 
    /// Esta clase implementa <see cref="ObservableObject"/>
    /// para permitir la actualización automática
    /// de la interfaz de usuario mediante el patrón MVVM.
    /// 
    /// Cada instancia almacena información relacionada con:
    /// - Número de serie.
    /// - Peso utilizado.
    /// - Repeticiones logradas.
    /// - Estado de finalización.
    /// </summary>
    public partial class SerieLogModel : ObservableObject
    {
        /// <summary>
        /// Número identificador de la serie
        /// dentro del ejercicio actual.
        /// </summary>
        public int NumeroSerie { get; set; }

        /// <summary>
        /// Peso utilizado durante la ejecución
        /// de la serie.
        /// 
        /// El valor predeterminado es 0.
        /// </summary>
        [ObservableProperty]
        private int _pesoUtilizado = 0;

        /// <summary>
        /// Cantidad de repeticiones logradas
        /// durante la serie.
        /// 
        /// El valor predeterminado es 0.
        /// </summary>
        [ObservableProperty]
        private int _repsLogradas = 0;

        /// <summary>
        /// Indica si la serie fue completada
        /// correctamente por el usuario.
        /// 
        /// El valor predeterminado es false.
        /// </summary>
        [ObservableProperty]
        private bool _completada = false;
    }
}