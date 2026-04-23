using RutinApp.Shared.Models;
using RutinApp.ViewModels;

namespace RutinApp.Views
{
    [QueryProperty("Rutina", "Rutina")]
    public partial class SesionPage : ContentPage
    {
        private readonly SesionViewModel _sesionViewModel;
        private RutinaModel _rutinaPendiente;

        public SesionPage(SesionViewModel sesionViewModel)
        {
            InitializeComponent();
            _sesionViewModel = sesionViewModel;
            BindingContext = _sesionViewModel;
        }

        public RutinaModel Rutina
        {
            set
            {
                if (value != null)
                    _rutinaPendiente = value;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_rutinaPendiente != null)
            {
                await _sesionViewModel.IniciarSesion(_rutinaPendiente);
                _rutinaPendiente = null;
            }
        }


    }
}