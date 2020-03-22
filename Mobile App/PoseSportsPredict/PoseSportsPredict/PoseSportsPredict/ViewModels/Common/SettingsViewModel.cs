using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Common
{
    internal class SettingsViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override void OnAppearing(params object[] datas)
        {
            base.OnAppearing(datas);
        }

        #endregion NavigableViewModel

        #region Constructors

        public SettingsViewModel(SettingsPage page) : base(page)
        {
            OnInitializeView();
        }

        #endregion Constructors
    }
}