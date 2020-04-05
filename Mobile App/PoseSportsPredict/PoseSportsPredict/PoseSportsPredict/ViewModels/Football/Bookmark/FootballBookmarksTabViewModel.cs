using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.CustomViews;
using PoseSportsPredict.Views.Football.Bookmark;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.ViewModels.Football.Bookmark
{
    public class FootballBookmarksTabViewModel : NavigableViewModel, IIconChange
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            IsSelected = false;

            var tabbedPage = this.CoupledPage as TabbedPage;

            tabbedPage.Children.Clear();

            tabbedPage.Children.Add(ShinyHost.Resolve<FootballBookmarkMatchesViewModel>().CoupledPage);
            tabbedPage.Children.Add(ShinyHost.Resolve<FootballBookmarkLeaguesViewModel>().CoupledPage);
            tabbedPage.Children.Add(ShinyHost.Resolve<FootballBookmarkTeamsViewModel>().CoupledPage);

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            IsSelected = true;

            if (this.CoupledPage is TabbedPage tabbedpage)
            {
                var bindingCtx = tabbedpage.CurrentPage.BindingContext as BaseViewModel;
                bindingCtx.OnAppearing();
            }
        }

        public override void OnDisAppearing(params object[] datas)
        {
            IsSelected = false;
        }

        #endregion NavigableViewModel

        #region IIconChange

        private bool _isSelected;

        public NavigationPage NavigationPage { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NavigationPage.IconImageSource = CurrentIcon;
                }
            }
        }

        public string CurrentIcon { get => IsSelected ? "ic_bookmark_selected.png" : "ic_bookmark_unselected.png"; }

        #endregion IIconChange

        #region Fields

        public bool _isSearchIconVisible;

        #endregion Fields

        #region Properties

        public bool IsSearchIconVisible { get => _isSearchIconVisible; set => SetValue(ref _isSearchIconVisible, value); }

        #endregion Properties

        #region Commands

        public ICommand SearchButtonClickCommand { get => new RelayCommand(SearchButtonClick); }

        private async void SearchButtonClick()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushPopupAsync(ShinyHost.Resolve<FootballBookmarkSearchViewModel>());

            SetIsBusy(false);
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

            NavigationPage = new MaterialNavigationPage(this.CoupledPage)
            {
                Title = LocalizeString.Bookmarks,
                IconImageSource = CurrentIcon,
            };

            if (OnInitializeView())
            {
                this.CoupledPage.Disappearing += (s, e) => OnDisAppearing();
            }

            ((TabbedPage)CoupledPage).CurrentPageChanged += (s, e) =>
            {
                if (s is TabbedPage curPage)
                {
                    var bindingCtx = curPage.BindingContext as BaseViewModel;
                    bindingCtx.OnAppearing();
                }
            };
        }

        #endregion Constructors
    }
}