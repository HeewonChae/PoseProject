using PoseSportsPredict.Resources;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football.Match;
using Shiny;
using System;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchesTabViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override void OnAppearing(params object[] datas)
        {
            var tabbedPage = this.CoupledPage as TabbedPage;

            if (tabbedPage.Children.Count > 0 && _curDate == DateTime.Now.Date)
                return;

            _curDate = DateTime.Now.Date;
            tabbedPage.Children.Clear();

            for (int i = -3; i <= 3; i++)
            {
                var matchesPage = ShinyHost.Resolve<FootballMatchesViewModel>().SetMatchDate(_curDate.AddDays(i)).CoupledPage;
                if (i == -1)
                {
                    matchesPage.Title = LocalizeString.Yesterday;
                }
                else if (i == 0)
                {
                    matchesPage.Title = LocalizeString.Today;
                }
                else if (i == 1)
                {
                    matchesPage.Title = LocalizeString.Tomorrow;
                }
                else
                {
                    matchesPage.Title = _curDate.AddDays(i).ToString("ddd MM/dd");
                }
                tabbedPage.Children.Add(matchesPage);
            }

            tabbedPage.CurrentPage = tabbedPage.Children[3]; // Today
        }

        #endregion NavigableViewModel

        #region Fields

        private DateTime _curDate;

        #endregion Fields

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

            OnInitializeView();
        }

        #endregion Constructors
    }
}