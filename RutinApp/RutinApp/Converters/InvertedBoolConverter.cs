namespace RutinApp.Converters
{
    /// <summary>
    /// Convertidor encargado de invertir valores booleanos.
    /// 
    /// Este converter es utilizado en enlaces XAML para
    /// cambiar dinámicamente el comportamiento de controles
    /// según el estado contrario de una propiedad booleana.
    /// 
    /// Ejemplo:
    /// true  -> false
    /// false -> true
    /// </summary>
    public class InvertedBoolConverter : IValueConverter
    {
        /// <summary>
        /// Convierte un valor booleano a su valor inverso.
        /// 
        /// Si el valor recibido es:
        /// true  -> retorna false
        /// false -> retorna true
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
        /// El valor booleano invertido.
        /// </returns>
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
            => value is bool b && !b;

        /// <summary>
        /// Realiza la conversión inversa del valor booleano.
        /// 
        /// Debido a que la inversión lógica es simétrica,
        /// este método aplica la misma lógica utilizada
        /// en el método <see cref="Convert"/>.
        /// </summary>
        /// <param name="value">
        /// Valor recibido para convertir de regreso.
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
        /// El valor booleano invertido.
        /// </returns>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            System.Globalization.CultureInfo culture)
            => value is bool b && !b;
    }
}