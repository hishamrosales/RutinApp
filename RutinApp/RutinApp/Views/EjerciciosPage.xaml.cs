using RutinApp.ViewModels;

/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.5
/// </summary>
namespace RutinApp.Views;

/// <summary>
/// Página encargada de mostrar el catálogo
/// de ejercicios registrados dentro
/// de la aplicación.
/// 
/// Esta vista permite al usuario:
/// - Visualizar todos los ejercicios creados.
/// - Consultar información general
///   de cada ejercicio.
/// - Acceder a opciones de edición
///   o eliminación.
/// - Navegar hacia la pantalla
///   de creación de nuevos ejercicios.
/// 
/// Toda la información mostrada
/// es administrada mediante
/// el ViewModel correspondiente.
/// </summary>
public partial class EjerciciosPage : ContentPage
{
    /// <summary>
    /// Instancia del ViewModel encargado
    /// de administrar la lógica de negocio
    /// relacionada con los ejercicios.
    /// </summary>
    private readonly EjercicioViewModel _ejercicioViewModel;

    /// <summary>
    /// Inicializa los componentes visuales
    /// de la página y establece el contexto
    /// de datos que permitirá enlazar
    /// la información del ViewModel
    /// con la interfaz gráfica.
    /// </summary>
    /// <param name="ejercicioViewModel">
    /// Instancia del ViewModel encargado
    /// de administrar la colección,
    /// comandos y operaciones relacionadas
    /// con los ejercicios registrados.
    /// </param>
    public EjerciciosPage(EjercicioViewModel ejercicioViewModel)
    {
        InitializeComponent();

        // Asigna la instancia del ViewModel
        // recibida mediante inyección
        // de dependencias.
        _ejercicioViewModel = ejercicioViewModel;

        // Establece el contexto de datos
        // para permitir el uso de
        // Data Binding con la interfaz XAML.
        BindingContext = _ejercicioViewModel;
    }

    /// <summary>
    /// Método ejecutado automáticamente
    /// cada vez que la página aparece
    /// en pantalla.
    /// 
    /// Su función es solicitar al ViewModel
    /// la carga de los ejercicios almacenados
    /// en la base de datos, asegurando
    /// que la información mostrada
    /// se encuentre actualizada.
    /// </summary>
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Solicita al ViewModel la carga
        // de los ejercicios disponibles.
        await _ejercicioViewModel.GetEjerciciosAsync();
    }
}