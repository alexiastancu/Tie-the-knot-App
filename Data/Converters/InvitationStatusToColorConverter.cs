using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Data.Enums;

namespace Wedding_Planning_App.Data.Converters
{
    public class InvitationStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is InvitationStatus status)
            {
                return status switch
                {
                    InvitationStatus.Accepted => Color.FromArgb("#a5d6a7"),
                    InvitationStatus.Rejected => Color.FromArgb("#ef9a9a"),
                    InvitationStatus.Pending => Color.FromArgb("#E5FFDA"),
                    _ => Colors.LightGray
                };
            }
            return Colors.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
