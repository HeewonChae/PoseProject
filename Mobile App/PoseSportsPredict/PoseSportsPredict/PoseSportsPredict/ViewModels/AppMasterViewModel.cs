using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.Views;
using Shiny;

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

        public override void OnAppearing(params object[] datas)
        {
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