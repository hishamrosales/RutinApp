using Microsoft.Maui.Controls;

/// <summary>
/// Autores: Hisham Rosales, Debanhi Librado, Jesus Guerrero
/// Fecha: 15/05/2026
/// Versión: 1.2
/// </summary>
namespace RutinApp.Converters
{
    /// <summary>
    /// Convertidor encargado de transformar un valor booleano
    /// en un color utilizado dentro de la interfaz gráfica.
    /// 
    /// Este converter es utilizado principalmente en XAML
    /// para cambiar dinámicamente el color de elementos
    /// según el estado de una tarea o rutina.
    /// 
    /// Ejemplo:
    /// true  = color de completado.
    /// false = color por defecto.
    /// </summary>
    public class BoolToColorConverter : IValueConverter
    {
        /// <summary>
        /// Convierte un valor booleano en un objeto <see cref="Color"/>.
        /// 
        /// Si el valor es verdadero, retorna un color verde oscuro
        /// indicando que la tarea fue completada.
        /// 
        /// Si el valor es falso, retorna un color oscuro por defecto.
        /// </summary>
        /// <param name="value">
        /// Valor recibido desde el binding.
        /// </param>
        /// <param name="targetType">
        /// Tipo de destino esperado para la conversión.
        /// </param>
        /// <param name="parameter">
        /// Parámetro opcional utilizado durante la conversión.
        /// </param>
        /// <param name="culture">
        /// Información cultural utilizada en la conversión.
        /// </param>
        /// <returns>
        /// Un objeto <see cref="Color"/> correspondiente
        /// al estado booleano recibido.
        /// </returns>
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (value is bool completada && completada)
                return Color.FromArgb("#1A3A2A");

            return Color.FromArgb("#2A2A3E");
        }

        /// <summary>
        /// Método no implementado para conversión inversa.
        /// 
        /// La aplicación únicamente requiere conversión
        /// de booleano a color, por lo que la conversión
        /// inversa no es necesaria.
        /// </summary>
        /// <param name="value">
        /// Valor a convertir de regreso.
        /// </param>
        /// <param name="targetType">
        /// Tipo de destino esperado.
        /// </param>
        /// <param name="parameter">
        /// Parámetro opcional utilizado durante la conversión.
        /// </param>
        /// <param name="culture">
        /// Información cultural utilizada en la conversión.
        /// </param>
        /// <returns>
        /// Este método no retorna ningún valor debido a que
        /// no se encuentra implementado.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Se lanza cuando se intenta realizar una conversión inversa.
        /// </exception>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
            => throw new NotImplementedException();
    }
}