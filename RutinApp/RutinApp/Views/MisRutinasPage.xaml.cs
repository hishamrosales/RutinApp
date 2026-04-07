using RutinApp.ViewModels;

namespace RutinApp.Views;

public partial class MisRutinasPage : ContentPage
{
    private readonly RutinaViewModel _rutinaViewModel;

    public MisRutinasPage(RutinaViewModel rutinaViewModel)
    {
        InitializeComponent();
        _rutinaViewModel = rutinaViewModel;
        BindingContext = _rutinaViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _rutinaViewModel.GetRutinasAsync();
    }
}