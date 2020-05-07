using Acr.UserDialogs;
using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Match;
using Shiny;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchesTabViewModel : NavigableViewModel, IIconChange
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            IsSelected = false;
            _curDate = DateTime.Now.Date;

            var tabbedPage = this.CoupledPage as TabbedPage;
            tabbedPage.Children.Clear();

            for (int i = -3; i <= 3; i++)
            {
                tabbedPage.Children.Add(
                    ShinyHost.Resolve<FootballMatchesViewModel>()
                    .SetMatchDate(_curDate.AddDays(i)).CoupledPage);
            }

            tabbedPage.CurrentPage = tabbedPage.Children[3]; // Today

            return base.OnInitializeView(datas);
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

        public string CurrentIcon { get => IsSelected ? "ic_stadium_selected.png" : "ic_stadium_unselected.png"; }

        #endregion IIconChange

        #region Fields

        private DateTime _curDate;

        #endregion Fields

        #region Commands

        public ICommand SelectMatchDateCommand { get => new RelayCommand(SelectMatchDate); }

        private async void SelectMatchDate()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var selectedDate = await UserDialogs.Instance.DatePromptAsync(null, DateTime.Now);
            if (selectedDate.Ok)
            {
                var tabbedPage = this.CoupledPage as TabbedPage;

                for (int i = -3; i <= 3; i++)
                {
                    var bindingCtx = tabbedPage.Children[i + 3].BindingContext as FootballMatchesViewModel;
                    bindingCtx.SetMatchDate(selectedDate.Value.AddDays(i));
                }

                tabbedPage.CurrentPage = tabbedPage.Children[3]; // selectedDate
                ((FootballMatchesViewModel)tabbedPage.CurrentPage.BindingContext).OnAppearing();
            }

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public FootballMatchesTabViewModel() : base(null)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SetCoupledPage(new FootballMatchesTabPage_IOS());
                    break;

                case Device.Android:
                    SetCoupledPage(new FootballMatchesTabPage());
                    break;
            }

            NavigationPage = new MaterialNavigationPage(this.CoupledPage)
            {
                Title = LocalizeString.Matches,
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
                     var bindingCtx = curPage.CurrentPage.BindingContext as BaseViewModel;
                     bindingCtx.OnAppearing();
                 }
             };
        }

        #endregion Constructors
    }
}