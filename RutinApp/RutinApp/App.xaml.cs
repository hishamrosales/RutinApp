using Microsoft.Extensions.DependencyInjection;

namespace RutinApp
{
    /// <summary>
    /// Clase principal de la aplicación RutinApp.
    /// 
    /// Esta clase hereda de <see cref="Application"/>
    /// y se encarga de:
    /// - Inicializar los recursos globales.
    /// - Configurar la ventana principal.
    /// - Definir el punto de inicio de la aplicación.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="App"/>.
        /// 
        /// Ejecuta la inicialización de componentes
        /// definidos en App.xaml.
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Crea y configura la ventana principal
        /// de la aplicación.
        /// 
        /// La aplicación utiliza <see cref="AppShell"/>
        /// como contenedor principal de navegación.
        /// </summary>
        /// <param name="activationState">
        /// Estado de activación de la aplicación.
        /// </param>
        /// <returns>
        /// Instancia de <see cref="Window"/>
        /// utilizada como ventana principal.
        /// </returns>
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}