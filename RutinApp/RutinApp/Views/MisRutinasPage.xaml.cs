using RutinApp.ViewModels;

/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.5
/// </summary>
namespace RutinApp.Views;

/// <summary>
/// Página encargada de mostrar todas las rutinas
/// registradas dentro de la aplicación.
/// 
/// Esta vista permite al usuario:
/// - Visualizar sus rutinas creadas.
/// - Consultar información general de cada rutina.
/// - Acceder a opciones de edición o eliminación.
/// - Iniciar una sesión de entrenamiento.
/// - Navegar hacia la creación de nuevas rutinas.
/// 
/// La información mostrada es administrada
/// mediante el ViewModel correspondiente.
/// </summary>
public partial class MisRutinasPage : ContentPage
{
    /// <summary>
    /// Instancia del ViewModel encargado de
    /// administrar la lógica de negocio
    /// relacionada con las rutinas.
    /// </summary>
    private readonly RutinaViewModel _rutinaViewModel;

    /// <summary>
    /// Inicializa los componentes visuales
    /// de la página y establece el contexto
    /// de datos necesario para realizar
    /// el enlace entre la interfaz gráfica
    /// y el ViewModel.
    /// </summary>
    /// <param name="rutinaViewModel">
    /// Instancia del ViewModel que contiene
    /// la lógica, comandos y colección
    /// de rutinas registradas.
    /// </param>
    public MisRutinasPage(RutinaViewModel rutinaViewModel)
    {
        InitializeComponent();

        // Asigna la instancia del ViewModel
        // recibida mediante inyección
        // de dependencias.
        _rutinaViewModel = rutinaViewModel;

        // Establece el contexto de datos
        // para permitir el uso de
        // Data Binding con la interfaz XAML.
        BindingContext = _rutinaViewModel;
    }

    /// <summary>
    /// Método ejecutado automáticamente
    /// cada vez que la página aparece
    /// en pantalla.
    /// 
    /// Su función es solicitar al ViewModel
    /// la carga de las rutinas registradas
    /// en la base de datos, garantizando
    /// que la información mostrada
    /// se encuentre actualizada.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Solicita al ViewModel la carga
        // de las rutinas disponibles.
        await _rutinaViewModel.GetRutinasAsync();
    }
}