using RutinApp.Shared.Models;
using RutinApp.ViewModels;

namespace RutinApp.Views
{
    [QueryProperty("Rutina", "Rutina")]
    public partial class SesionPage : ContentPage
    {
        private readonly SesionViewModel _sesionViewModel;

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
                    _sesionViewModel.IniciarSesion(value);
            }
        }
    }
}