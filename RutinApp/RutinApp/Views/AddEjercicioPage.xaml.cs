using RutinApp.ViewModels;
using RutinApp.Shared.Models;

/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.5
/// </summary>
namespace RutinApp.Views
{
    /// <summary>
    /// Página encargada de registrar nuevos ejercicios
    /// o editar ejercicios previamente existentes
    /// dentro de la aplicación.
    /// 
    /// Esta vista permite:
    /// - Capturar el nombre del ejercicio.
    /// - Registrar el grupo muscular correspondiente.
    /// - Agregar una descripción del ejercicio.
    /// - Editar información previamente almacenada.
    /// - Limpiar automáticamente el formulario
    ///   cuando se crea un nuevo registro.
    /// 
    /// Toda la lógica de negocio y validaciones
    /// son administradas mediante el ViewModel
    /// correspondiente.
    /// </summary>
    [QueryProperty("Ejercicio", "Ejercicio")]
    public partial class AddEjercicioPage : ContentPage
    {
        /// <summary>
        /// Instancia del ViewModel encargado
        /// de administrar la lógica de negocio,
        /// validaciones y operaciones relacionadas
        /// con los ejercicios.
        /// </summary>
        private readonly EjercicioViewModel _ejercicioViewModel;

        /// <summary>
        /// Variable utilizada para almacenar
        /// temporalmente el ejercicio recibido
        /// mediante navegación.
        /// 
        /// Esta referencia permite determinar
        /// si la página se encuentra en modo
        /// creación o edición.
        /// </summary>
        private EjercicioModel _ejercicio;

        /// <summary>
        /// Inicializa los componentes visuales
        /// de la página y establece el contexto
        /// de datos necesario para enlazar
        /// la interfaz gráfica con el ViewModel.
        /// </summary>
        /// <param name="ejercicioViewModel">
        /// Instancia del ViewModel encargado
        /// de administrar la lógica,
        /// validaciones y operaciones
        /// relacionadas con los ejercicios.
        /// </param>
        public AddEjercicioPage(EjercicioViewModel ejercicioViewModel)
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
        /// Si no existe un ejercicio cargado,
        /// significa que la vista está en modo
        /// creación, por lo que el formulario
        /// es reiniciado.
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Verifica si la vista se encuentra
            // en modo creación.
            if (_ejercicio == null)
                _ejercicioViewModel.LimpiarFormulario();
        }

        /// <summary>
        /// Método ejecutado automáticamente
        /// cuando la página deja de estar
        /// visible en pantalla.
        /// 
        /// Su función es limpiar la referencia
        /// del ejercicio actual para evitar
        /// reutilizar información anterior
        /// en futuras aperturas.
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Limpia la referencia actual
            // del ejercicio seleccionado.
            _ejercicio = null;
        }

        /// <summary>
        /// Propiedad utilizada por Shell Navigation
        /// para recibir un ejercicio desde
        /// otra vista de la aplicación.
        /// 
        /// Cuando se recibe un ejercicio válido,
        /// este se almacena localmente y se envía
        /// al ViewModel para cargar la información
        /// correspondiente en modo edición.
        /// </summary>
        public EjercicioModel Ejercicio
        {
            get => _ejercicio;

            set
            {
                // Almacena el ejercicio recibido.
                _ejercicio = value;

                // Verifica que exista un ejercicio
                // válido antes de cargar
                // la información en el ViewModel.
                if (_ejercicioViewModel != null && value != null)
                    _ejercicioViewModel.LoadEjercicioForEdicion(value);
            }
        }
    }
}