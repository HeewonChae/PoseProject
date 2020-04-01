using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.KeyboardHelper;

namespace PoseSportsPredict.Views.Football.League
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballLeaguesPage : ContentPage
    {
        private double _lastScorllY;

        public FootballLeaguesPage()
        {
            InitializeComponent();
        }

        private void lv_matches_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (_lastScorllY != e.ScrollY)
            {
                _lastScorllY = e.ScrollY;

                if (_searchBar.IsFocused)
                    _searchBar.Unfocus();
            }
        }
    }
}