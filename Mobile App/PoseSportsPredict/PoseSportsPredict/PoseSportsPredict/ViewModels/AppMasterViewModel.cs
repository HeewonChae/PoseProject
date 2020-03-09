using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.Views;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.ViewModels
{
    public class AppMasterViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override async Task<bool> PrepareView(params object[] data)
        {
            var masterPate = CoupledPage as AppMasterPage;

            if (!await _menuViewModel.PrepareView(ShinyHost.Resolve<FootballTabbedViewModel>()))
                return false;

            masterPate.Master = _menuViewModel.CoupledPage;
            masterPate.Detail = _defaultDetailViewModel.CoupledPage;

            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private AppMasterMenuViewModel _menuViewModel;
        private BaseViewModel _defaultDetailViewModel;

        #endregion Fields

        #region Constructors

        public AppMasterViewModel(AppMasterPage page) : base(page)
        {
            _menuViewModel = ShinyHost.Resolve<AppMasterMenuViewModel>();
        }

        #endregion Constructors
    }
}