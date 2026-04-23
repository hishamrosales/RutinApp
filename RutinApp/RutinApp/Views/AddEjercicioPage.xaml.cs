using RutinApp.ViewModels;
using RutinApp.Shared.Models;

namespace RutinApp.Views
{
    [QueryProperty("Ejercicio", "Ejercicio")]
    public partial class AddEjercicioPage : ContentPage
    {
        private readonly EjercicioViewModel _ejercicioViewModel;
        private EjercicioModel _ejercicio;

        public AddEjercicioPage(EjercicioViewModel ejercicioViewModel)
        {
            InitializeComponent();
            _ejercicioViewModel = ejercicioViewModel;
            BindingContext = _ejercicioViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_ejercicio == null)
                _ejercicioViewModel.LimpiarFormulario();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _ejercicio = null;
        }

        public EjercicioModel Ejercicio
        {
            get => _ejercicio;
            set
            {
                _ejercicio = value;
                if (_ejercicioViewModel != null && value != null)
                    _ejercicioViewModel.LoadEjercicioForEdicion(value);
            }
        }
    }
}