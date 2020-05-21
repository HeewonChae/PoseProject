using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common.Detail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class VIPClubViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            return await base.OnPrepareViewAsync(datas);
        }

        #endregion NavigableViewModel

        #region Constructors

        public VIPClubViewModel(VipClubPage page) : base(page)
        {
        }

        #endregion Constructors
    }
}