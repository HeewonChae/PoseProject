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

        public override bool OnApearing(params object[] datas)
        {
            return true;
        }

        #endregion NavigableViewModel

        #region Constructors

        public FootballMatchesViewModel(FootballMatchesPage page) : base(page)
        {
        }

        #endregion Constructors
    }
}