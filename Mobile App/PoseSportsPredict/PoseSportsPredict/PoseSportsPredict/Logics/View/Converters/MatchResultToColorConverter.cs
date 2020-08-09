using PosePacket.Service.Enum;
using PoseSportsPredict.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class MatchResultToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color returnValue = Color.Transparent;

            if (value is MatchResultType resultType)
            {
                switch (resultType)
                {
                    case MatchResultType.Win:
                        returnValue = AppResourcesHelper.GetResourceColor("WinColor");
                        break;

                    case MatchResultType.Draw:
                        returnValue = AppResourcesHelper.GetResourceColor("CustomGrey");
                        break;

                    case MatchResultType.Lose:
                        returnValue = AppResourcesHelper.GetResourceColor("LoseColor");
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