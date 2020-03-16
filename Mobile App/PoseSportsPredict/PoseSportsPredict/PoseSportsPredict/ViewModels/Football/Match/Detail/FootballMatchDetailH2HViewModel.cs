using PoseSportsPredict.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMatchDetailH2HViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            base.OnAppearing(datas);
        }

        #endregion BaseViewModel

        #region Constructors

        public FootballMatchDetailH2HViewModel()
        {
            OnInitializeView();
        }

        #endregion Constructors
    }
}