using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMatchDetailViewModel : NavigableViewModel
    {
        #region Constructors
        public FootballMatchDetailViewModel(FootballMatchDetailPage page) : base(page)
        {
            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => OnAppearing();
            }
        }
        #endregion
    }
}