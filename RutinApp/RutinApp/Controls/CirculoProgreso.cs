/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.1
/// </summary>
namespace RutinApp.Controls
{
    /// <summary>
    /// Control personalizado reutilizable que representa un círculo de progreso.
    /// 
    /// Este control hereda de <see cref="GraphicsView"/> y utiliza la clase
    /// <see cref="CirculoProgresoDrawable"/> para renderizar visualmente el progreso.
    /// 
    /// La actualización de propiedades se realiza mediante <see cref="BindableProperty"/>,
    /// permitiendo enlazar datos desde ViewModels siguiendo el patrón MVVM.
    /// 
    /// Ejemplo de uso en XAML:
    /// xmlns:controls="clr-namespace:RutinApp.Controls"
    /// 
    /// <controls:CirculoProgreso
    ///     Progreso="{Binding ProgresoSesion}"
    ///     Texto="{Binding PorcentajeSesionTexto}" />
    /// </summary>
    public class CirculoProgreso : GraphicsView
    {
        private readonly CirculoProgresoDrawable _drawable;

        /// <summary>
        /// Propiedad enlazable que almacena el valor del progreso del círculo.
        /// 
        /// El valor esperado debe estar entre 0.0 y 1.0,
        /// donde:
        /// 0.0 = 0%
        /// 1.0 = 100%
        /// 
        /// Cuando el valor cambia, el control se redibuja automáticamente.
        /// </summary>
        public static readonly BindableProperty ProgresoProperty =
            BindableProperty.Create(
                nameof(Progreso),
                typeof(double),
                typeof(CirculoProgreso),
                0.0,
                propertyChanged: (bindable, _, newVal) =>
                {
                    var ctrl = (CirculoProgreso)bindable;
                    ctrl._drawable.Progreso = (float)(double)newVal;
                    ctrl.Invalidate();
                });

        /// <summary>
        /// Propiedad enlazable que define el texto mostrado
        /// en el centro del círculo de progreso.
        /// 
        /// Ejemplo:
        /// "45%"
        /// </summary>
        public static readonly BindableProperty TextoProperty =
            BindableProperty.Create(
                nameof(Texto),
                typeof(string),
                typeof(CirculoProgreso),
                "0%",
                propertyChanged: (bindable, _, newVal) =>
                {
                    var ctrl = (CirculoProgreso)bindable;
                    ctrl._drawable.Texto = (string)newVal;
                    ctrl.Invalidate();
                });

        /// <summary>
        /// Obtiene o establece el progreso actual del control.
        /// 
        /// Este valor puede enlazarse desde un ViewModel utilizando MVVM.
        /// </summary>
        public double Progreso
        {
            get => (double)GetValue(ProgresoProperty);
            set => SetValue(ProgresoProperty, value);
        }

        /// <summary>
        /// Obtiene o establece el texto central mostrado
        /// dentro del círculo de progreso.
        /// </summary>
        public string Texto
        {
            get => (string)GetValue(TextoProperty);
            set => SetValue(TextoProperty, value);
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="CirculoProgreso"/>.
        /// 
        /// Se crea una instancia del drawable encargado del renderizado visual
        /// y se asigna a la propiedad <see cref="Drawable"/> del control.
        /// </summary>
        public CirculoProgreso()
        {
            _drawable = new CirculoProgresoDrawable();
            Drawable = _drawable;
        }
    }
}