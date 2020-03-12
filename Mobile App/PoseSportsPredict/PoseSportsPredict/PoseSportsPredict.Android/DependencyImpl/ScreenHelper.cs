using PoseSportsPredict.Droid.DependencyImpl;
using PoseSportsPredict.InfraStructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenHelper))]

namespace PoseSportsPredict.Droid.DependencyImpl
{
    public class ScreenHelper : IScreenHelper
    {
        #region Fields

        private double _displayScaleFactor;
        private Size _screenSize;
        private Thickness _safeAreaInsets;

        #endregion Fields

        public double DisplayScaleFactor => _displayScaleFactor;
        public Size ScreenSize => _screenSize;
        public Thickness SafeAreaInsets => _safeAreaInsets;

        public ScreenSizeType ScreenSizeType
        {
            get
            {
                if (_screenSize.Width <= 384)
                {
                    return ScreenSizeType.Small;
                }

                if (_screenSize.Width <= 540)
                {
                    return ScreenSizeType.Regular;
                }

                return ScreenSizeType.Big;
            }
        }

        public ScreenHelper()
        {
            _displayScaleFactor = Android.Content.Res.Resources.System.DisplayMetrics.Density;

            var width = (int)(Android.Content.Res.Resources.System.DisplayMetrics.WidthPixels / _displayScaleFactor);
            var height = (int)(Android.Content.Res.Resources.System.DisplayMetrics.HeightPixels / _displayScaleFactor);

            if (width > height)
            {
                var temp = width;
                width = height;
                height = temp;
            }

            _screenSize = new Size(width, height);

            _safeAreaInsets = new Thickness()
            {
                Left = 0,
                Top = 0,
                Right = 0,
                Bottom = 0,
            };
        }

        public int DpToPixels(int dp) => (int)(DisplayScaleFactor * dp);

        public int DpToPixels(double dp) => (int)(DisplayScaleFactor * dp);
    }
}