using System;
using System.Globalization;
using Xamarin.Forms;
namespace ExhibitorModule.Controls.Converters
{
    public class GravatarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!int.TryParse(parameter?.ToString(), out int size))
                size = 76;
            
            return $"{value.ToString()}?s={size}&d=mp";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
