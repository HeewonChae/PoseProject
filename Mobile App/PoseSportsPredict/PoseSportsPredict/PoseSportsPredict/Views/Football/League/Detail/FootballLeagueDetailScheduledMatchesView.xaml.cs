using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.League.Detail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballLeagueDetailScheduledMatchesView : ContentView
    {
        public FootballLeagueDetailScheduledMatchesView()
        {
            InitializeComponent();
        }

        private void _AdMob_AdsLoaded(object sender, EventArgs e)
        {
            _AdMob.IsVisible = true;
        }
    }
}