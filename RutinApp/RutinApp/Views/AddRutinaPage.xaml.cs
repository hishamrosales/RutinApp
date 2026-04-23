using RutinApp.ViewModels;
using RutinApp.Shared.Models;

namespace RutinApp.Views
{
    [QueryProperty("Rutina", "Rutina")]
    public partial class AddRutinaPage : ContentPage
    {
        private readonly RutinaViewModel _rutinaViewModel;
        private RutinaModel _rutina;

        public AddRutinaPage(RutinaViewModel rutinaViewModel)
        {
            InitializeComponent();
            _rutinaViewModel = rutinaViewModel;
            BindingContext = _rutinaViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_rutina == null)
                _rutinaViewModel.LimpiarFormulario();
            await _rutinaViewModel.GetEjerciciosDisponiblesAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _rutina = null;
        }

        public RutinaModel Rutina
        {
            get => _rutina;
            set
            {
                _rutina = value;
                if (_rutinaViewModel != null && value != null)
                    _rutinaViewModel.LoadRutinaForEdicion(value);
            }
        }
    }
}