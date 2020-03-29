using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class MatchAlarmColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isAlarmed = (bool)value;

            return isAlarmed ? Color.Gold : AppResourcesHelper.GetResourceColor("CustomGrey_L");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (Color)value;

            return color == Color.Gold ? true : false;
        }
    }
}