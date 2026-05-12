using Microsoft.Maui.Controls;

namespace RutinApp.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool completada && completada)
                return Color.FromArgb("#1A3A2A"); 
            return Color.FromArgb("#2A2A3E");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            => throw new NotImplementedException();
    }
}