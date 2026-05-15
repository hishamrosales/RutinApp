/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.2
/// </summary>
namespace RutinApp
{
    /// <summary>
    /// Clase principal de navegación de la aplicación.
    /// 
    /// Esta clase hereda de <see cref="Shell"/> y se encarga
    /// de configurar la estructura de navegación global
    /// de la aplicación utilizando .NET MAUI Shell.
    /// 
    /// Aquí se registran las rutas utilizadas para
    /// la navegación entre páginas mediante navegación
    /// basada en rutas.
    /// </summary>
    public partial class AppShell : Shell
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="AppShell"/>.
        /// 
        /// Se encarga de:
        /// - Inicializar los componentes visuales.
        /// - Registrar las rutas de navegación utilizadas
        ///   en la aplicación.
        /// </summary>
        public AppShell()
        {
            InitializeComponent();

            // Registro de ruta para la página
            // de agregar o editar ejercicios.
            Routing.RegisterRoute(
                nameof(Views.AddEjercicioPage),
                typeof(Views.AddEjercicioPage));

            // Registro de ruta para la página
            // de agregar o editar rutinas.
            Routing.RegisterRoute(
                nameof(Views.AddRutinaPage),
                typeof(Views.AddRutinaPage));

            // Registro de ruta para la página
            // de sesión de entrenamiento.
            Routing.RegisterRoute(
                nameof(Views.SesionPage),
                typeof(Views.SesionPage));
        }
    }
}