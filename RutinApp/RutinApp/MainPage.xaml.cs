namespace RutinApp
{
    /// <summary>
    /// Página principal de ejemplo generada por defecto
    /// en el proyecto .NET MAUI.
    /// 
    /// Esta página contiene un contador simple que incrementa
    /// su valor cada vez que el usuario presiona un botón.
    /// 
    /// Su funcionalidad principal es demostrar:
    /// - Manejo de eventos.
    /// - Actualización de interfaz.
    /// - Accesibilidad mediante lector de pantalla.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Contador utilizado para registrar
        /// la cantidad de veces que el botón fue presionado.
        /// </summary>
        int count = 0;

        /// <summary>
        /// Inicializa una nueva instancia de la clase
        /// <see cref="MainPage"/>.
        /// 
        /// Ejecuta la inicialización de los componentes
        /// definidos en el archivo XAML asociado.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Método ejecutado cuando el usuario presiona
        /// el botón del contador.
        /// 
        /// Incrementa el valor del contador y actualiza
        /// el texto mostrado en pantalla.
        /// 
        /// También utiliza accesibilidad mediante
        /// <see cref="SemanticScreenReader"/> para anunciar
        /// el nuevo valor al lector de pantalla.
        /// </summary>
        /// <param name="sender">
        /// Objeto que originó el evento.
        /// </param>
        /// <param name="e">
        /// Datos asociados al evento.
        /// </param>
        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}