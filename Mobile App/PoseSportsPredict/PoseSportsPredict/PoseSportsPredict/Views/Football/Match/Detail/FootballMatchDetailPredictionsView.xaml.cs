using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using Shiny;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Match.Detail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMatchDetailPredictionsView : ContentView
    {
        public FootballMatchDetailPredictionsView()
        {
            InitializeComponent();
        }

        private void _AdMob_AdsLoaded()
        {
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