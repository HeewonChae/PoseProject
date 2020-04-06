using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Utilities.LocalStorage;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using PoseSportsPredict.Views;
using Shiny;
using System.Linq;
using System.Threading.Tasks;
using WebServiceShare.ServiceContext;
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
            masterPage.Detail = ShinyHost.Resolve<AppMasterMenuViewModel>().SportsCategories.First().SourcePage;

            return true;
        }

        public override Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            var masterPage = this.CoupledPage as AppMasterPage;
            masterPage.IsPresented = false;

            ShinyHost.Resolve<AppMasterMenuViewModel>().RefrashUserInfo();
            ShinyHost.Resolve<AppMasterMenuViewModel>().LastLoginTime = ClientContext.LastLoginTime;

            return Task.FromResult(true);
        }

        public override async void OnAppearing(params object[] datas)
        {
            if (this.CoupledPage is AppMasterPage masterPage)
            {
                var bindingCtx = masterPage.Detail.BindingContext as BaseViewModel;
                bindingCtx.OnAppearing();
            }
        }

        #endregion NavigableViewModel

        #region Constructors

        public AppMasterViewModel(AppMasterPage page) : base(page)
        {
            if (OnInitializeView())
            {
                CoupledPage.Appearing += (s, e) => this.OnAppearing();
            }
        }

        #endregion Constructors
    }
}