using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Bookmark
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballBookmarkSearchPage : ContentPage
    {
        public FootballBookmarkSearchPage()
        {
            InitializeComponent();

            Xamarin.Forms.Application.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
        }
    }
}