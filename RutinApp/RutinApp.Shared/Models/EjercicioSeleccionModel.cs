using CommunityToolkit.Mvvm.ComponentModel;

namespace RutinApp.Shared.Models
{
    public partial class EjercicioSeleccionModel : ObservableObject
    {
        public EjercicioModel Ejercicio { get; set; } = new();

        [ObservableProperty]
        private bool _isSelected = false;

        [ObservableProperty]
        private int _series = 3;

        [ObservableProperty]
        private int _repeticiones = 10;
    }
}