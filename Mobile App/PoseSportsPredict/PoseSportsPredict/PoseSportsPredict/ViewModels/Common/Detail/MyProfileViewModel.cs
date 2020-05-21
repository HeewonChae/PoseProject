using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common.Detail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class MyProfileViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            return await base.OnPrepareViewAsync(datas);
        }

        #endregion NavigableViewModel

        #region Constructors

        public MyProfileViewModel(MyProfilePage page) : base(page)
        {
        }

        #endregion Constructors
    }
}