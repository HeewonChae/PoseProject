using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Bookmark;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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
            tabbedPage.Children.Add(ShinyHost.Resolve<FootballBookmarkLeaguesViewModel>().CoupledPage);
            tabbedPage.Children.Add(ShinyHost.Resolve<FootballBookmarkTeamsViewModel>().CoupledPage);

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            var tabbedPage = this.CoupledPage as TabbedPage;
            var bindingCtx = tabbedPage.CurrentPage.BindingContext as NavigableViewModel;
            bindingCtx.OnAppearing();
        }

        #endregion NavigableViewModel

        #region Fields

        public bool _isSearchIconVisible;

        #endregion Fields

        #region Properties

        public bool IsSearchIconVisible { get => _isSearchIconVisible; set => SetValue(ref _isSearchIconVisible, value); }

        #endregion Properties

        #region Commands

        public ICommand SearchButtonClickCommand { get => new RelayCommand(SearchButtonClick); }

        private void SearchButtonClick()
        {
        }

        #endregion Commands

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

            // Workaround NavigationPage OnAppearing bug
            ((TabbedPage)this.CoupledPage).CurrentPageChanged += (s, e) =>
            {
                if (s is TabbedPage tabbedPage)
                {
                    var bindingCtx = tabbedPage.CurrentPage.BindingContext as BaseViewModel;
                    bindingCtx.OnAppearing();
                }
            };
        }

        #endregion Constructors
    }
}