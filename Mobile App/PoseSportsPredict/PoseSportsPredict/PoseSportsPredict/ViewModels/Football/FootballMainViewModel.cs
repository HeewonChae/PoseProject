using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match;
using PoseSportsPredict.Views.Football;
using Shiny;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMainViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override void OnAppearing(params object[] datas)
        {
            var mainPage = this.CoupledPage as FootballMainPage;

            if (mainPage.Children.Count > 0)
                return;

            var matchesPage = ShinyHost.Resolve<FootballMatchesTabViewModel>().CoupledPage;
            var matchesNavPage = new MaterialNavigationPage(matchesPage)
            {
                Title = matchesPage.Title,
                IconImageSource = matchesPage.IconImageSource,
            };
            mainPage.Children.Add(matchesNavPage);

            var leaugesPage = ShinyHost.Resolve<FootballLeaguesViewModel>().CoupledPage;
            var leaguesNavPage = new MaterialNavigationPage(leaugesPage)
            {
                Title = leaugesPage.Title,
                IconImageSource = leaugesPage.IconImageSource,
            };
            mainPage.Children.Add(leaguesNavPage);
        }

        #endregion NavigableViewModel

        #region Constructors

        public FootballMainViewModel(FootballMainPage page) : base(page)
        {
            // Workaround NavigationPage OnAppearing bug
            page.CurrentPageChanged += (s, e) =>
            {
                if (s is TabbedPage tabbedPage
                && tabbedPage.CurrentPage is NavigationPage navPage)
                {
                    var bindingCtx = navPage.CurrentPage.BindingContext as BaseViewModel;
                    bindingCtx.OnAppearing();
                }
            };

            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }
        }

        #endregion Constructors
    }
}