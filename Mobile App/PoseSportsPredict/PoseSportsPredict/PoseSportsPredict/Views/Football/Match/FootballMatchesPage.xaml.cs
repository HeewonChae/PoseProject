using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Football.Match
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballMatchesPage : ContentPage
    {
        public FootballMatchesPage()
        {
            InitializeComponent();
        }

        private void _coverFlow_UserInteracted(PanCardView.CardsView view, PanCardView.EventArgs.UserInteractedEventArgs args)
        {
            if (args.Status == PanCardView.Enums.UserInteractionStatus.Started)
            {
                FootballMatchesTabPage.DisableSwipe();
            }
            if (args.Status == PanCardView.Enums.UserInteractionStatus.Ended)
            {
                FootballMatchesTabPage.EnableSwipe();
            }
        }
    }
}