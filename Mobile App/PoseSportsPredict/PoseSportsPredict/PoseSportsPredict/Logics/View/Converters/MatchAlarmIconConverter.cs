using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    internal class MatchAlarmIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isAlarmed = (bool)value;

            return isAlarmed ? "ic_alarm_selected.png" : "ic_alarm_unselected.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var icon = (string)value;

            return icon.Equals("ic_alarm_selected") ? true : false;
        }
    }
}