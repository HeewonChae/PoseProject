using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class FootballMatchStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var matchStatus = value.ToString();

            matchStatus.TryParseEnum(out FootballMatchStatusType statusType);

            if (statusType == FootballMatchStatusType.NS)
                return LocalizeString.Match_Not_Start;
            else if (statusType == FootballMatchStatusType.FT
                || statusType == FootballMatchStatusType.AET
                || statusType == FootballMatchStatusType.PEN)
                return LocalizeString.Match_Finished;

            return LocalizeString.Match_Ongoing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}