using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Views.Football;
using Shiny;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballTabbedViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override async Task<bool> PrepareView(params object[] data)
        {
            if (!await _matchesViewModel.PrepareView()
                || !await _leaguesViewModel.PrepareView())
                return false;

            TabbedPage tabbedPage = CoupledPage as TabbedPage;

            // Matches
            tabbedPage.Children.Add(new NavigationPage(_matchesViewModel.CoupledPage)
            {
                Title = _matchesViewModel.CoupledPage.Title,
                IconImageSource = _matchesViewModel.CoupledPage.IconImageSource,
            });

            // Leagues
            tabbedPage.Children.Add(new NavigationPage(_leaguesViewModel.CoupledPage)
            {
                Title = _leaguesViewModel.CoupledPage.Title,
                IconImageSource = _leaguesViewModel.CoupledPage.IconImageSource,
            });

            tabbedPage.CurrentPage = _matchesViewModel.CoupledPage;

            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private FootballMatchesViewModel _matchesViewModel;
        private FootballLeaguesViewModel _leaguesViewModel;

        #endregion Fields

        #region Constructors

        public FootballTabbedViewModel(FootballTabbedPage page) : base(page)
        {
            _matchesViewModel = ShinyHost.Resolve<FootballMatchesViewModel>();
            _leaguesViewModel = ShinyHost.Resolve<FootballLeaguesViewModel>();
        }

        #endregion Constructors
    }
}