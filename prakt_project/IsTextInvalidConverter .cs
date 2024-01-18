using System;
using System.Globalization;
using System.Windows.Data;
using System.Linq;
namespace prakt_project
{
    public class IsTextInvalidConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Проверка на некорректные данные (в данном случае, наличие символов, не являющихся буквами)
            if (value is string text)
            {
                return !string.IsNullOrEmpty(text) && !text.All(char.IsLetter);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
