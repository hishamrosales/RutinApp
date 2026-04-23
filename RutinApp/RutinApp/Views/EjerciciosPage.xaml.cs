using RutinApp.ViewModels;

namespace RutinApp.Views;

public partial class EjerciciosPage : ContentPage
{
    private readonly EjercicioViewModel _ejercicioViewModel;

    public EjerciciosPage(EjercicioViewModel ejercicioViewModel)
    {
        InitializeComponent();
        _ejercicioViewModel = ejercicioViewModel;
        BindingContext = _ejercicioViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _ejercicioViewModel.GetEjerciciosAsync();
    }
}