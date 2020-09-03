using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using Shiny;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Match.Detail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMatchDetailH2HView : ContentView
    {
        public FootballMatchDetailH2HView()
        {
            InitializeComponent();
        }

        private void _AdMob_AdsLoaded(object sender, System.EventArgs e)
        {
            var membershipType = ShinyHost.Resolve<MembershipService>().MemberRoleType;
            if (MembershipAdvantage.TryGetValue(membershipType, out MembershipAdvantage advantage))
            {
                _AdMob.IsVisible = !advantage.IsBannerAdRemove;
            }
        }
    }
}