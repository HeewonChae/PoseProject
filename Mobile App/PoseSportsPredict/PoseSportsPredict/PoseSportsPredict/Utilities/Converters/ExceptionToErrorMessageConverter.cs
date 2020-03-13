using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Utilities.Converters
{
    public class ExceptionToErrorMessageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var exception = value as Exception;

            if (value == null)
            {
                return null;
            }

            return exception.Message;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // One-Way converter only
            throw new NotImplementedException();
        }
    }
}