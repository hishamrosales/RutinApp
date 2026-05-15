using RutinApp.ViewModels;

/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.4
/// </summary>
namespace RutinApp.Views;

/// <summary>
/// Página encargada de mostrar el historial
/// de sesiones de entrenamiento realizadas
/// por el usuario dentro de la aplicación.
/// 
/// Esta vista permite:
/// - Consultar entrenamientos finalizados.
/// - Visualizar la fecha de realización.
/// - Mostrar información resumida
///   de cada sesión registrada.
/// 
/// La información es obtenida desde
/// la base de datos y administrada
/// mediante el ViewModel correspondiente.
/// </summary>
public partial class HistorialPage : ContentPage
{
    /// <summary>
    /// Instancia del ViewModel encargado
    /// de administrar la lógica de negocio
    /// relacionada con el historial
    /// de entrenamientos.
    /// </summary>
    private readonly HistorialViewModel _viewModel;

    /// <summary>
    /// Inicializa los componentes visuales
    /// de la página y establece el contexto
    /// de datos que permitirá enlazar
    /// la información del ViewModel
    /// con la interfaz gráfica.
    /// </summary>
    /// <param name="viewModel">
    /// Instancia del ViewModel encargado
    /// de obtener, administrar y exponer
    /// la información del historial
    /// de entrenamientos.
    /// </param>
    public HistorialPage(HistorialViewModel viewModel)
    {
        InitializeComponent();

        // Asigna la instancia del ViewModel
        // recibida mediante inyección
        // de dependencias.
        _viewModel = viewModel;

        // Establece el contexto de datos
        // para permitir el uso de
        // Data Binding con XAML.
        BindingContext = _viewModel;
    }

    /// <summary>
    /// Método ejecutado automáticamente
    /// cada vez que la página aparece
    /// en pantalla.
    /// 
    /// Su función es solicitar al ViewModel
    /// la carga del historial almacenado
    /// en la base de datos, asegurando
    /// que la información mostrada
    /// se encuentre actualizada.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Solicita al ViewModel la carga
        // del historial de entrenamientos.
        await _viewModel.LoadHistorialAsync();
    }
}