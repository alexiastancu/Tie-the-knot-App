using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wedding_Planning_App.Data.Converters
{
    public class SelectedSeatToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int selectedIndex = (int)value;
            int seatIndex = int.Parse(parameter.ToString());
            return selectedIndex == seatIndex ? Colors.Green : Colors.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
