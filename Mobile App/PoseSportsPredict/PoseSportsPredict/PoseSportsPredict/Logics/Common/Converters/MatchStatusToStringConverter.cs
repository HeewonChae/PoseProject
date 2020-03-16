using PoseSportsPredict.Resources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Common.Converters
{
    public class MatchStatusToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var matchStatus = value.ToString();

            if (matchStatus.Equals("NS"))
                return LocalizeString.Match_Not_Start;
            else if (matchStatus.Equals("FT")
                || matchStatus.Equals("AET")
                || matchStatus.Equals("PEN"))
                return LocalizeString.Match_Finished;

            return LocalizeString.Match_Ongoing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}