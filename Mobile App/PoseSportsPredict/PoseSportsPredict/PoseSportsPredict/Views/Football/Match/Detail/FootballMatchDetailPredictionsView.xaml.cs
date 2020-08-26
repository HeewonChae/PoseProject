using PoseSportsPredict.ViewModels.Football.Match.Detail;

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
            this._AdMob.IsVisible = true;
        }
    }
}