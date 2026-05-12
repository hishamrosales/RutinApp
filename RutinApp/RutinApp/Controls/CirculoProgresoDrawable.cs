namespace RutinApp.Controls
{
    /// <summary>
    /// IDrawable que pinta un arco de progreso circular.
    /// Úsalo como Drawable de un GraphicsView.
    /// </summary>
    public class CirculoProgresoDrawable : IDrawable
    {
        // Valor entre 0.0 y 1.0
        public float Progreso { get; set; } = 0f;
        public string Texto { get; set; } = "0%";

        // Colores
        public Color ColorFondo { get; set; } = Color.FromArgb("#2A2A3E");
        public Color ColorArco { get; set; } = Color.FromArgb("#FF6B35");
        public Color ColorTexto { get; set; } = Colors.White;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float cx = dirtyRect.Width / 2f;
            float cy = dirtyRect.Height / 2f;
            float grosor = 7f;
            float radio = Math.Min(cx, cy) - grosor / 2f - 2f;

            // Arco de fondo (círculo completo gris)
            canvas.StrokeColor = ColorFondo;
            canvas.StrokeSize = grosor;
            canvas.DrawEllipse(cx - radio, cy - radio, radio * 2, radio * 2);

            // Arco de progreso
            if (Progreso > 0f)
            {
                canvas.StrokeColor = ColorArco;
                canvas.StrokeSize = grosor;
                canvas.StrokeLineCap = LineCap.Round;

                // Ángulo: empieza arriba (-90°), sentido horario
                float startAngle = -90f;
                float sweepAngle = 360f * Math.Clamp(Progreso, 0f, 1f);

                canvas.DrawArc(
                    cx - radio, cy - radio,
                    radio * 2, radio * 2,
                    startAngle,
                    startAngle + sweepAngle,
                    false, false);
            }

            // Texto central
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
