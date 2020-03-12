using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.iOS.DependencyImpl;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ScreenHelper))]

namespace PoseSportsPredict.iOS.DependencyImpl
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
                if (_screenSize.Width <= 320)
                {
                    return ScreenSizeType.Small;
                }

                if (_screenSize.Width <= 375)
                {
                    return ScreenSizeType.Regular;
                }

                return ScreenSizeType.Big;
            }
        }

        public ScreenHelper()
        {
            _displayScaleFactor = UIScreen.MainScreen.Scale;

            var width = (int)(UIScreen.MainScreen.Bounds.Width / _displayScaleFactor);
            var height = (int)(UIScreen.MainScreen.Bounds.Height / _displayScaleFactor);

            if (width > height)
            {
                var temp = width;
                width = height;
                height = temp;
            }

            _screenSize = new Size(width, height);

            var insets = UIScreen.MainScreen.OverscanCompensationInsets;
            _safeAreaInsets = new Thickness()
            {
                Left = insets.Left,
                Top = insets.Top,
                Right = insets.Right,
                Bottom = insets.Bottom,
            };
        }

        public int DpToPixels(int dp) => (int)(DisplayScaleFactor * dp);

        public int DpToPixels(double dp) => (int)(DisplayScaleFactor * dp);
    }
}