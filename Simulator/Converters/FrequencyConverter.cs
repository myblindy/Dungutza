using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Simulator.Converters
{
    class FrequencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is double hz
                ? hz > 1_000_000 ? $"{hz / 1_000_000:0.000} MHz"
                : hz > 1_000 ? $"{hz / 1_000:0.000} KHz"
                : $"{hz:0.000} Hz"
            : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
