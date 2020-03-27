using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Common;
using PoseSportsPredict.ViewModels.Football.Bookmark;
using PoseSportsPredict.ViewModels.Football.Match;
using PoseSportsPredict.Views.Football;
using Shiny;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMainViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            var mainPage = this.CoupledPage as FootballMainPage;

            var matchesPage = ShinyHost.Resolve<FootballMatchesTabViewModel>().CoupledPage;
            var matchesNavPage = new MaterialNavigationPage(matchesPage)
            {
                Title = matchesPage.Title,
                IconImageSource = "ic_stadium.png",
                BarBackgroundColor = Color.FromHex("#1FBED6"),
                BarTextColor = Color.FromHex("#000000")
            };
            mainPage.Children.Add(matchesNavPage);

            var leaugesPage = ShinyHost.Resolve<FootballLeaguesViewModel>().CoupledPage;
            var leaguesNavPage = new MaterialNavigationPage(leaugesPage)
            {
                Title = leaugesPage.Title,
                IconImageSource = "ic_trophy.png",
            };
            mainPage.Children.Add(leaguesNavPage);

            var bookmarksPage = ShinyHost.Resolve<FootballBookmarksTabViewModel>().CoupledPage;
            var bookmarksNavPage = new MaterialNavigationPage(bookmarksPage)
            {
                Title = bookmarksPage.Title,
                IconImageSource = "ic_bookmark.png",
            };
            mainPage.Children.Add(bookmarksNavPage);

            var settingsPage = ShinyHost.Resolve<SettingsViewModel>().CoupledPage;
            var settingsNavPage = new MaterialNavigationPage(settingsPage)
            {
                Title = settingsPage.Title,
                IconImageSource = "ic_setting.png",
            };
            mainPage.Children.Add(settingsNavPage);

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
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

            OnInitializeView();
        }

        #endregion Constructors
    }
}