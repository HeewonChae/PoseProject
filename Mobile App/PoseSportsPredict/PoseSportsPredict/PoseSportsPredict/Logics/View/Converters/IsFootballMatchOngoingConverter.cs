using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class IsFootballMatchOngoingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FootballMatchInfo matchInfo)
            {
                bool isMatchFinished = false;
                if (matchInfo.MatchStatus == FootballMatchStatusType.FT
                    || matchInfo.MatchStatus == FootballMatchStatusType.AET
                    || matchInfo.MatchStatus == FootballMatchStatusType.PEN)
                    isMatchFinished = true;

                return matchInfo.MatchTime < DateTime.Now && !isMatchFinished;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now;
        }
    }
}