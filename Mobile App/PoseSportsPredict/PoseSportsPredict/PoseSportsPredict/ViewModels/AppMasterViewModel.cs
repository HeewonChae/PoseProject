using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.Views;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.ViewModels
{
    public class AppMasterViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnPrepareView(params object[] datas)
        {
            var masterPage = this.CoupledPage as AppMasterPage;

            masterPage.Master = _masterMenuViewModel.CoupledPage;
            masterPage.Detail = _defaultDetailViewModel.CoupledPage;

            return true;
        }

        #endregion NavigableViewModel

        #region Fields

        private AppMasterMenuViewModel _masterMenuViewModel;
        private NavigableViewModel _defaultDetailViewModel;

        #endregion Fields

        #region Constructors

        public AppMasterViewModel(AppMasterPage page) : base(page)
        {
            _masterMenuViewModel = ShinyHost.Resolve<AppMasterMenuViewModel>();
            _defaultDetailViewModel = ShinyHost.Resolve<FootballMainViewModel>();
        }

        #endregion Constructors
    }
}