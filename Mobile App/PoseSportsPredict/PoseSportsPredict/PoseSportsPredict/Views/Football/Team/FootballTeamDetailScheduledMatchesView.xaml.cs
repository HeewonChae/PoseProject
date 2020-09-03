using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Services;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Team
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballTeamDetailScheduledMatchesView : ContentView
    {
        public FootballTeamDetailScheduledMatchesView()
        {
            InitializeComponent();
        }

        private void _AdMob_AdsLoaded(object sender, EventArgs e)
        {
            var membershipType = ShinyHost.Resolve<MembershipService>().MemberRoleType;
            if (MembershipAdvantage.TryGetValue(membershipType, out MembershipAdvantage advantage))
            {
                _AdMob.IsVisible = !advantage.IsBannerAdRemove;
            }
        }
    }
}