using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMatchesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            return true;
        }

        public override bool OnAppearing(params object[] datas)
        {
            return true;
        }

        #endregion NavigableViewModel

        #region Fields

        private DateTime _matchDate;

        #endregion Fields

        #region Constructors

        public FootballMatchesViewModel(FootballMatchesPage page) : base(page)
        {
            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }
        }

        #endregion Constructors

        #region Methods

        public FootballMatchesViewModel SetMatchDate(DateTime date)
        {
            _matchDate = date;
            return this;
        }

        #endregion Methods
    }
}