using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.Views.Common.Ads
{
    public class AdmobSmallNativeAdsView : ContentView
    {
        //public static readonly BindableProperty ButtonColorProperty = BindableProperty.Create(
        //    nameof(ButtonColor),
        //    typeof(double),
        //    typeof(Color),
        //    Color.Accent);

        //public double ButtonColor { get { return (double)GetValue(ButtonColorProperty); } set { SetValue(ButtonColorProperty, value); } }

        public Color ButtonColor { get; set; }

        public AdmobSmallNativeAdsView()
        {
            this.IsVisible = false;
        }
    }
}