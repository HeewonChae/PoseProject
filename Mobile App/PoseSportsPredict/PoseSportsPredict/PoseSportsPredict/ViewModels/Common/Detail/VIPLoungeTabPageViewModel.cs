using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common.Detail;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class VIPLoungeTabPageViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            var tabbedPage = this.CoupledPage as TabbedPage;

            tabbedPage.Children.Clear();

            var matchesPage = ShinyHost.Resolve<VIPMatchesViewModel>().CoupledPage;
            var historyPage = ShinyHost.Resolve<VIPHistoryViewModel>().CoupledPage;
            var subscribePage = ShinyHost.Resolve<VIPSubscribeViewModel>().CoupledPage;

            tabbedPage.Children.Add(matchesPage);
            tabbedPage.Children.Add(historyPage);
            tabbedPage.Children.Add(subscribePage);

            if (_membershipService.MemberRoleType > PosePacket.Service.Enum.MemberRoleType.Regular)
                tabbedPage.CurrentPage = matchesPage;
            else
                tabbedPage.CurrentPage = subscribePage;

            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            if (this.CoupledPage is TabbedPage tabbedpage)
            {
                var bindingCtx = tabbedpage.CurrentPage.BindingContext as BaseViewModel;
                bindingCtx.OnAppearing();
            }
        }

        public override void OnDisAppearing(params object[] datas)
        {
        }

        #endregion NavigableViewModel

        #region Services

        private MembershipService _membershipService;

        #endregion Services

        #region Constructors

        public VIPLoungeTabPageViewModel(MembershipService membershipService) : base(null)
        {
            _membershipService = membershipService;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    SetCoupledPage(new VIPLoungeTabPage_IOS());
                    break;

                case Device.Android:
                    SetCoupledPage(new VIPLoungeTabPage());
                    break;
            }

            if (OnInitializeView())
            {
                this.CoupledPage.Appearing += (s, e) => OnAppearing();
                this.CoupledPage.Disappearing += (s, e) => OnDisAppearing();
            }

            ((TabbedPage)CoupledPage).CurrentPageChanged += (s, e) =>
            {
                if (s is TabbedPage curPage)
                {
                    var bindingCtx = curPage.CurrentPage.BindingContext as BaseViewModel;
                    bindingCtx.OnAppearing();
                }
            };
        }

        #endregion Constructors
    }
}