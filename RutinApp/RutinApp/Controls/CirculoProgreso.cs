namespace RutinApp.Controls
{
    /// <summary>
    /// Control reutilizable de círculo de progreso.
    /// Colócalo en la carpeta Controls/ del proyecto RutinApp.
    /// En XAML: xmlns:controls="clr-namespace:RutinApp.Controls"
    ///           <controls:CirculoProgreso Progreso="{Binding ProgresoSesion}" Texto="{Binding PorcentajeSesionTexto}" />
    /// </summary>
    public class CirculoProgreso : GraphicsView
    {
        private readonly CirculoProgresoDrawable _drawable;

        // BindableProperty: Progreso (0.0 – 1.0)
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

        // BindableProperty: Texto central (ej. "45%")
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

        public double Progreso
        {
            get => (double)GetValue(ProgresoProperty);
            set => SetValue(ProgresoProperty, value);
        }

        public string Texto
        {
            get => (string)GetValue(TextoProperty);
            set => SetValue(TextoProperty, value);
        }

        public CirculoProgreso()
        {
            _drawable = new CirculoProgresoDrawable();
            Drawable = _drawable;
        }
    }
}
