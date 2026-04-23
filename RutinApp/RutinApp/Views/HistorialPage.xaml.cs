using RutinApp.ViewModels;

namespace RutinApp.Views;

public partial class HistorialPage : ContentPage
{
    private readonly HistorialViewModel _viewModel;

    public HistorialPage(HistorialViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadHistorialAsync();
    }
}