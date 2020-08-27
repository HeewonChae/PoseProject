using PoseSportsPredict.ViewModels.Base;

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
            this._AdMob.IsVisible = true;
        }
    }
}