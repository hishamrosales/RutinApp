using CommunityToolkit.Mvvm.ComponentModel;

namespace RutinApp.Shared.Models
{
    /// <summary>
    /// Modelo utilizado para representar un ejercicio
    /// seleccionable dentro de una rutina.
    /// 
    /// Esta clase implementa <see cref="ObservableObject"/>
    /// para permitir la actualización automática de la interfaz
    /// mediante el patrón MVVM.
    /// 
    /// Además de almacenar el ejercicio seleccionado,
    /// también registra:
    /// - Estado de selección.
    /// - Número de series.
    /// - Número de repeticiones.
    /// </summary>
    public partial class EjercicioSeleccionModel : ObservableObject
    {
        /// <summary>
        /// Obtiene o establece el ejercicio asociado.
        /// </summary>
        public EjercicioModel Ejercicio { get; set; } = new();

        /// <summary>
        /// Indica si el ejercicio se encuentra seleccionado
        /// para formar parte de una rutina.
        /// </summary>
        [ObservableProperty]
        private bool _isSelected = false;

        /// <summary>
        /// Cantidad de series configuradas
        /// para el ejercicio.
        /// 
        /// El valor predeterminado es 3.
        /// </summary>
        [ObservableProperty]
        private int _series = 3;

        /// <summary>
        /// Cantidad de repeticiones configuradas
        /// para el ejercicio.
        /// 
        /// El valor predeterminado es 10.
        /// </summary>
        [ObservableProperty]
        private int _repeticiones = 10;
    }
}