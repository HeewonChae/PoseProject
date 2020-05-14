using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Utilities;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class IsMatchNotStartedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var matchTime = (DateTime)value;

            return matchTime > DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now;
        }
    }
}