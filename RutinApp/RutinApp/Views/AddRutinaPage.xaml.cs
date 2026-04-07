using RutinApp.ViewModels;
using RutinApp.Shared.Models;

namespace RutinApp.Views;

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

    public RutinaModel Rutina
    {
        get => _rutina;
        set
        {
            _rutina = value;
            if (_rutinaViewModel != null && value != null)
                _rutinaViewModel.LoadRutinaForEdition(value);
        }
    }
}