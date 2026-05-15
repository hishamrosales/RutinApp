using RutinApp.ViewModels;
using RutinApp.Shared.Models;

/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.6
/// </summary>
namespace RutinApp.Views
{
    /// <summary>
    /// Página encargada de registrar nuevas rutinas
    /// o editar rutinas previamente existentes
    /// dentro de la aplicación.
    /// 
    /// Esta vista permite:
    /// - Capturar el nombre de una rutina.
    /// - Seleccionar ejercicios disponibles.
    /// - Editar información de una rutina existente.
    /// - Cargar automáticamente ejercicios almacenados.
    /// - Restablecer el formulario cuando sea necesario.
    /// 
    /// La lógica de negocio es administrada
    /// mediante el ViewModel correspondiente.
    /// </summary>
    [QueryProperty("Rutina", "Rutina")]
    public partial class AddRutinaPage : ContentPage
    {
        /// <summary>
        /// Instancia del ViewModel encargado
        /// de administrar la lógica de negocio
        /// relacionada con la creación
        /// y edición de rutinas.
        /// </summary>
        private readonly RutinaViewModel _rutinaViewModel;

        /// <summary>
        /// Variable utilizada para almacenar
        /// temporalmente la rutina recibida
        /// mediante navegación.
        /// 
        /// Esta referencia permite determinar
        /// si la vista se encuentra en modo
        /// creación o edición.
        /// </summary>
        private RutinaModel _rutina;

        /// <summary>
        /// Inicializa los componentes visuales
        /// de la página y establece el contexto
        /// de datos necesario para enlazar
        /// la interfaz gráfica con el ViewModel.
        /// </summary>
        /// <param name="rutinaViewModel">
        /// Instancia del ViewModel encargado
        /// de administrar la lógica,
        /// validaciones y operaciones
        /// relacionadas con las rutinas.
        /// </param>
        public AddRutinaPage(RutinaViewModel rutinaViewModel)
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
        /// Si no existe una rutina cargada,
        /// el formulario es limpiado para
        /// iniciar un nuevo registro.
        /// 
        /// Posteriormente se cargan los ejercicios
        /// disponibles desde la base de datos
        /// para permitir su selección.
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Verifica si la vista se encuentra
            // en modo creación.
            if (_rutina == null)
                _rutinaViewModel.LimpiarFormulario();

            // Solicita al ViewModel la carga
            // de ejercicios disponibles.
            await _rutinaViewModel.GetEjerciciosDisponiblesAsync();
        }

        /// <summary>
        /// Método ejecutado automáticamente
        /// cuando la página deja de estar
        /// visible en pantalla.
        /// 
        /// Su función es limpiar la referencia
        /// de la rutina actual para evitar
        /// reutilizar información anterior
        /// en futuras aperturas.
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Limpia la referencia actual
            // de la rutina seleccionada.
            _rutina = null;
        }

        /// <summary>
        /// Propiedad utilizada por Shell Navigation
        /// para recibir una rutina desde
        /// otra vista de la aplicación.
        /// 
        /// Cuando se recibe una rutina válida,
        /// esta se asigna localmente y se envía
        /// al ViewModel para cargar
        /// la información correspondiente
        /// en modo edición.
        /// </summary>
        public RutinaModel Rutina
        {
            get => _rutina;

            set
            {
                // Almacena la rutina recibida.
                _rutina = value;

                // Verifica que exista una rutina
                // válida antes de cargar
                // la información en el ViewModel.
                if (_rutinaViewModel != null && value != null)
                    _rutinaViewModel.LoadRutinaForEdicion(value);
            }
        }
    }
}