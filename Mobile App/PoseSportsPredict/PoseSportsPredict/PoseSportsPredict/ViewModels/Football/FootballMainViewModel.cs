using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Common;
using PoseSportsPredict.ViewModels.Football.Bookmark;
using PoseSportsPredict.ViewModels.Football.League;
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

            mainPage.Children.Add(ShinyHost.Resolve<FootballMatchesTabViewModel>().NavigationPage);
            mainPage.Children.Add(ShinyHost.Resolve<FootballLeaguesViewModel>().NavigationPage);
            mainPage.Children.Add(ShinyHost.Resolve<FootballBookmarksTabViewModel>().NavigationPage);
            mainPage.Children.Add(ShinyHost.Resolve<SettingsViewModel>().NavigationPage);

            return true;
        }

        public override Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            if (this.CoupledPage is TabbedPage tabbedPage)
            {
                tabbedPage.CurrentPage = tabbedPage.Children[0];
            }
            return base.OnPrepareViewAsync(datas);
        }

        public override void OnAppearing(params object[] datas)
        {
            if (this.CoupledPage is TabbedPage tabbedpage
                && tabbedpage.CurrentPage is NavigationPage navPage)
            {
                var bindingCtx = navPage.CurrentPage.BindingContext as BaseViewModel;
                bindingCtx.OnAppearing();
            }
        }

        #endregion NavigableViewModel

        #region Constructors

        public FootballMainViewModel(FootballMainPage page) : base(page)
        {
            OnInitializeView();

            page.CurrentPageChanged += (s, e) =>
            {
                if (s is TabbedPage tabbedpage
                && tabbedpage.CurrentPage is NavigationPage navPage)
                {
                    var bindingCtx = navPage.CurrentPage.BindingContext as BaseViewModel;
                    bindingCtx.OnAppearing();
                }
            };
        }

        #endregion Constructors
    }
}