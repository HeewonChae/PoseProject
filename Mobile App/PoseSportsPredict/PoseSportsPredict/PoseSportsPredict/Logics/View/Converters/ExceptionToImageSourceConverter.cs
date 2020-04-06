using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.Converters
{
    public class ExceptionToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            string imageName;
            switch (value)
            {
                default:
                    imageName = "img_problem.png";
                    break;
            }

            return ImageSource.FromFile(imageName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}