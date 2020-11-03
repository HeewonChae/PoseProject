using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common.Detail;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class VIPHistoryViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
        }

        #endregion NavigableViewModel

        #region Constructors

        public VIPHistoryViewModel(
            VIPHistoryPage page) : base(page)
        {
            OnInitializeView();
        }

        #endregion Constructors
    }
}