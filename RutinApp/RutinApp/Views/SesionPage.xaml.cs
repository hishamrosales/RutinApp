using RutinApp.Shared.Models;
using RutinApp.ViewModels;

/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.4
/// </summary>
namespace RutinApp.Views
{
    /// <summary>
    /// Página encargada de administrar y mostrar
    /// la sesión activa de entrenamiento.
    /// 
    /// Esta vista recibe una rutina seleccionada
    /// desde la pantalla de rutinas y la envía
    /// al ViewModel correspondiente para iniciar
    /// la sesión de entrenamiento.
    /// 
    /// También se encarga de:
    /// - Recibir parámetros mediante navegación.
    /// - Asignar la rutina seleccionada.
    /// - Inicializar la sesión al abrir la vista.
    /// </summary>
    [QueryProperty("Rutina", "Rutina")]
    public partial class SesionPage : ContentPage
    {
        /// <summary>
        /// Instancia del ViewModel encargado de
        /// administrar toda la lógica de negocio
        /// relacionada con la sesión de entrenamiento.
        /// </summary>
        private readonly SesionViewModel _sesionViewModel;

        /// <summary>
        /// Variable temporal utilizada para almacenar
        /// la rutina recibida mediante navegación,
        /// antes de ser procesada por el ViewModel.
        /// </summary>
        private RutinaModel _rutinaPendiente;

        /// <summary>
        /// Inicializa los componentes visuales de la página
        /// y establece el contexto de datos para permitir
        /// la comunicación entre la interfaz y el ViewModel.
        /// </summary>
        /// <param name="sesionViewModel">
        /// Instancia del ViewModel encargado de
        /// controlar la lógica de la sesión activa.
        /// </param>
        public SesionPage(SesionViewModel sesionViewModel)
        {
            InitializeComponent();

            // Asigna el ViewModel recibido mediante
            // inyección de dependencias.
            _sesionViewModel = sesionViewModel;

            // Establece el BindingContext para permitir
            // el uso de Data Binding con XAML.
            BindingContext = _sesionViewModel;
        }

        /// <summary>
        /// Propiedad utilizada por Shell Navigation
        /// para recibir la rutina seleccionada
        /// desde otra vista de la aplicación.
        /// 
        /// Cuando se recibe una rutina válida,
        /// esta se almacena temporalmente para
        /// iniciar la sesión posteriormente.
        /// </summary>
        public RutinaModel Rutina
        {
            set
            {
                // Verifica que la rutina recibida
                // no sea nula antes de almacenarla.
                if (value != null)
                    _rutinaPendiente = value;
            }
        }

        /// <summary>
        /// Método ejecutado automáticamente cada vez
        /// que la página aparece en pantalla.
        /// 
        /// Si existe una rutina pendiente por cargar,
        /// se envía al ViewModel para inicializar
        /// la sesión de entrenamiento correspondiente.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Verifica si existe una rutina
            // pendiente de inicialización.
            if (_rutinaPendiente != null)
            {
                // Inicia la sesión utilizando
                // la rutina recibida.
                await _sesionViewModel.IniciarSesion(_rutinaPendiente);

                // Limpia la referencia para evitar
                // reinicializaciones accidentales.
                _rutinaPendiente = null;
            }
        }


    }
}