using PoseSportsPredict.Logics;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.Resources;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace PoseSportsPredict
{
    public class DialogConfiguration
    {
        public static readonly MaterialAlertDialogConfiguration DefaultAlterDialogConfiguration = new MaterialAlertDialogConfiguration
        {
            BackgroundColor = Color.White,
            TitleTextColor = AppResourcesHelper.GetResource<Color>("PrimaryColor"),
            MessageTextColor = AppResourcesHelper.GetResource<Color>("CustomGrey_D"),
            TintColor = AppResourcesHelper.GetResource<Color>("PrimaryColor"),
            CornerRadius = 5,
            ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32),
            ButtonAllCaps = true
        };

        public static readonly MaterialAlertDialogConfiguration AppTitleAlterDialogConfiguration = new MaterialAlertDialogConfiguration
        {
            BackgroundColor = Color.White,
            TitleTextColor = AppResourcesHelper.GetResource<Color>("TextColor_D2"),
            MessageTextColor = AppResourcesHelper.GetResource<Color>("CustomGrey_D"),
            TintColor = AppResourcesHelper.GetResource<Color>("PrimaryColor"),
            CornerRadius = 5,
            ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32),
            TitleFontFamily = "AppTitleFont_B",
            ButtonAllCaps = true
        };

        public static readonly MaterialSimpleDialogConfiguration DefaultSimpleDialogConfiguration = new MaterialSimpleDialogConfiguration
        {
            BackgroundColor = Color.White,
            TitleTextColor = AppResourcesHelper.GetResource<Color>("PrimaryColor"),
            TextColor = AppResourcesHelper.GetResource<Color>("TextColor_D"),
            TintColor = AppResourcesHelper.GetResource<Color>("PrimaryColor"),
            CornerRadius = 5,
            ScrimColor = Color.FromHex("#232F34").MultiplyAlpha(0.32),
        };
    }
}