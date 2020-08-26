using MarcTron.Plugin.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Bookmark
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballBookmarkMatchesPage : ContentPage
    {
        public FootballBookmarkMatchesPage()
        {
            InitializeComponent();
        }

        private void MTAdView_AdsLoaded(object sender, EventArgs e)
        {
            _AdMob.IsVisible = true;
        }
    }
}