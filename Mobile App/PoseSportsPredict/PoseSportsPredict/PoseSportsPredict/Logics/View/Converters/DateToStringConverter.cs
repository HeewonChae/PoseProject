using PoseSportsPredict.Resources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var localTime = ((DateTime)value).ToLocalTime();

            if (localTime.Date == DateTime.Today)
            {
                return LocalizeString.Today;
            }
            else if (localTime.Date == DateTime.Today.AddDays(-1))
            {
                return LocalizeString.Yesterday;
            }
            else if (localTime.Date == DateTime.Today.AddDays(1))
            {
                return LocalizeString.Tomorrow;
            }

            return localTime.ToString("ddd MM/dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = DateTime.TryParse(value.ToString(), out DateTime result);

            return ret ? result : DateTime.Now;
        }
    }
}