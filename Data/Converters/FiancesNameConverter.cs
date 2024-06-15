using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wedding_Planning_App.Services.Interfaces;

namespace Wedding_Planning_App.Data.Converters
{
    public class FiancesNameConverter : IValueConverter
    {
        private readonly IFiancesService _fiancesService;

        public FiancesNameConverter()
        {
            _fiancesService = MauiProgram.CreateMauiApp().Services.GetRequiredService<IFiancesService>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int fiancesId)
            {
                var fiances = Task.Run(() => _fiancesService.GetFiancesByIdAsync(fiancesId)).Result;
                if (fiances != null)
                {
                    return $"{fiances.HusbandName} & {fiances.WifeName}'s wedding";
                }
            }
            return "Unknown Fiances";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

