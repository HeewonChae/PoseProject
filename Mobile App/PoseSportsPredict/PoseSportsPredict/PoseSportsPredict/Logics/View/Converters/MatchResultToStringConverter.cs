using PosePacket.Service.Enum;
using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class MatchResultToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string returnValue = "";

            if (value is MatchResultType resultType)
            {
                switch (resultType)
                {
                    case MatchResultType.Win:
                        returnValue = "W";
                        break;

                    case MatchResultType.Draw:
                        returnValue = "D";
                        break;

                    case MatchResultType.Lose:
                        returnValue = "L";
                        break;

                    default:
                        break;
                }
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}