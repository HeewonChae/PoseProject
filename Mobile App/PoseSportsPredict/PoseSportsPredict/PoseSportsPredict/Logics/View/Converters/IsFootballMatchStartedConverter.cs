using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Utilities;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class IsFootballMatchStartedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var matchStatus = value.ToString();

            matchStatus.TryParseEnum(out FootballMatchStatusType statusType);

            if (statusType == FootballMatchStatusType.NS)
                return false;

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}