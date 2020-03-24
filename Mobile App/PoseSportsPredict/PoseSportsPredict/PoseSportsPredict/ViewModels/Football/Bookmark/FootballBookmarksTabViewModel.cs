using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Bookmark;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarksTabViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            var tabbedPage = this.CoupledPage as TabbedPage;

            tabbedPage.Children.Clear();

            tabbedPage.Children.Add(ShinyHost.Resolve<FootballBookmarkMatchesViewModel>().CoupledPage);
            tabbedPage.Children.Add(ShinyHost.Resolve<FootballBookmarkTeamsViewModel>().CoupledPage);
            tabbedPage.Children.Add(ShinyHost.Resolve<FootballBookmarkLeaguesViewModel>().CoupledPage);

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            base.OnAppearing(datas);
        }

        #endregion NavigableViewModel

        #region Constructors

        public FootballBookmarksTabViewModel() : base(null)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SetCoupledPage(new FootballBookmarksTabPage_IOS());
                    break;

                case Device.Android:
                    SetCoupledPage(new FootballBookmarksTabPage());
                    break;
            }

            OnInitializeView();
        }

        #endregion Constructors
    }
}