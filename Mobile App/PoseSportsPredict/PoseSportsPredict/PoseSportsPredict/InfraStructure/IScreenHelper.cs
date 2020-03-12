using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.InfraStructure
{
    public enum ScreenSizeType
    {
        Regular = 0,
        Small = 1,
        Big = 2,
    }

    public interface IScreenHelper
    {
        double DisplayScaleFactor { get; }

        Size ScreenSize { get; }

        ScreenSizeType ScreenSizeType { get; }

        Thickness SafeAreaInsets { get; }

        int DpToPixels(int dp);

        int DpToPixels(double dp);
    }
}