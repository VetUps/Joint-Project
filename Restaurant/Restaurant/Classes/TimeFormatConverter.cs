using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Restaurant.Classes
{
    internal class TimeFormatConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
                return parameter switch
                {
                    "Date" => dateTime.ToLongDateString(), // Дата
                    "Time" => dateTime.ToShortTimeString(), // Время
                    _ => dateTime.ToString() // По умолчанию
                };

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
