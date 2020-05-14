using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class IsMatchStartedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var matchTime = (DateTime)value;

            return matchTime < DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now;
        }
    }
}