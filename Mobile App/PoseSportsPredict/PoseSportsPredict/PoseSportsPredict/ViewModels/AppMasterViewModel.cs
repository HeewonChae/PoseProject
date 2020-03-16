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

        public override bool OnInitializeView(params object[] datas)
        {
            var masterPage = this.CoupledPage as AppMasterPage;

            masterPage.Master = ShinyHost.Resolve<AppMasterMenuViewModel>().CoupledPage;
            masterPage.Detail = ShinyHost.Resolve<FootballMainViewModel>().CoupledPage;

            return true;
        }

        #endregion NavigableViewModel

        #region Constructors

        public AppMasterViewModel(AppMasterPage page) : base(page)
        {
            CoupledPage.Appearing += (s, e) => OnAppearing();
        }

        #endregion Constructors
    }
}