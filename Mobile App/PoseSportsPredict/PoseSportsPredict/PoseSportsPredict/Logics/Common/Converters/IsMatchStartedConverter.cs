using System;
using System.Globalization;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Common.Converters
{
    public class IsMatchStartedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var matchStatus = value.ToString();

            if (matchStatus.Equals("NS"))
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}