namespace RutinApp.Controls
{
    /// <summary>
    /// Clase encargada de dibujar visualmente un círculo de progreso
    /// utilizando la interfaz <see cref="IDrawable"/>.
    /// 
    /// Esta clase es utilizada por el control personalizado
    /// <see cref="CirculoProgreso"/> para renderizar:
    /// - El círculo de fondo.
    /// - El arco de progreso.
    /// - El texto central.
    /// 
    /// La lógica de dibujo se mantiene separada del control visual
    /// para respetar la modularidad y facilitar el mantenimiento.
    /// </summary>
    public class CirculoProgresoDrawable : IDrawable
    {
        /// <summary>
        /// Obtiene o establece el valor del progreso mostrado.
        /// 
        /// El rango esperado es entre 0f y 1f:
        /// 0f = 0%
        /// 1f = 100%
        /// </summary>
        public float Progreso { get; set; } = 0f;

        /// <summary>
        /// Obtiene o establece el texto que se mostrará
        /// en el centro del círculo de progreso.
        /// 
        /// Ejemplo:
        /// "75%"
        /// </summary>
        public string Texto { get; set; } = "0%";

        /// <summary>
        /// Obtiene o establece el color utilizado para
        /// el círculo de fondo.
        /// </summary>
        public Color ColorFondo { get; set; } = Color.FromArgb("#2A2A3E");

        /// <summary>
        /// Obtiene o establece el color utilizado para
        /// representar el progreso del círculo.
        /// </summary>
        public Color ColorArco { get; set; } = Color.FromArgb("#FF6B35");

        /// <summary>
        /// Obtiene o establece el color utilizado
        /// para el texto central.
        /// </summary>
        public Color ColorTexto { get; set; } = Colors.White;

        /// <summary>
        /// Dibuja el contenido gráfico del círculo de progreso.
        /// 
        /// Este método renderiza:
        /// - El círculo base.
        /// - El arco correspondiente al progreso actual.
        /// - El texto central.
        /// </summary>
        /// <param name="canvas">
        /// Superficie gráfica sobre la que se realizará el dibujo.
        /// </param>
        /// <param name="dirtyRect">
        /// Área disponible para renderizar el contenido.
        /// </param>
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float cx = dirtyRect.Width / 2f;
            float cy = dirtyRect.Height / 2f;
            float grosor = 7f;
            float radio = Math.Min(cx, cy) - grosor / 2f - 2f;

            // Dibuja el círculo de fondo completo.
            canvas.StrokeColor = ColorFondo;
            canvas.StrokeSize = grosor;
            canvas.DrawEllipse(cx - radio, cy - radio, radio * 2, radio * 2);

            // Dibuja el arco de progreso únicamente
            // si el valor de progreso es mayor a cero.
            if (Progreso > 0f)
            {
                canvas.StrokeColor = ColorArco;
                canvas.StrokeSize = grosor;
                canvas.StrokeLineCap = LineCap.Round;

                // Limita el valor del progreso entre 0 y 1.
                float clampado = Math.Clamp(Progreso, 0f, 1f);

                // Si el progreso es del 100%,
                // se dibuja un círculo completo.
                if (clampado >= 1f)
                {
                    canvas.DrawEllipse(cx - radio, cy - radio, radio * 2, radio * 2);
                }
                else
                {
                    float startAngle = 270f;
                    float endAngle = startAngle + (360f * clampado);

                    // Dibuja el arco correspondiente
                    // al porcentaje de progreso actual.
                    canvas.DrawArc(
                        cx - radio, cy - radio,
                        radio * 2, radio * 2,
                        startAngle,
                        endAngle,
                        true,
                        false);
                }
            }

            // Dibuja el texto centrado dentro del círculo.
            canvas.FontColor = ColorTexto;
            canvas.FontSize = 15f;

            canvas.DrawString(
                Texto,
                dirtyRect,
                HorizontalAlignment.Center,
                VerticalAlignment.Center);
        }
    }
}