using GalaSoft.MvvmLight.Command;
using Plugin.InAppBilling.Abstractions;
using PosePacket.Service.Billing.Models.Enums;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common.Detail;
using Shiny;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class VIPClubViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override async Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            return await base.OnPrepareViewAsync(datas);
        }

        public override bool OnInitializeView(params object[] datas)
        {
            VIPProduct = _inAppBillingService.SubscriptionProduct[0];
            DiamondProduct = _inAppBillingService.InAppProduct[0];

            return true;
        }

        #endregion NavigableViewModel

        #region Service

        private InAppBillingService _inAppBillingService;
        private MembershipService _membershipService;

        #endregion Service

        #region Fields

        private InAppBillingProduct _VIPProduct;
        private InAppBillingProduct _DiamondProduct;

        #endregion Fields

        #region Properties

        public InAppBillingProduct VIPProduct { get => _VIPProduct; set => SetValue(ref _VIPProduct, value); }
        public InAppBillingProduct DiamondProduct { get => _DiamondProduct; set => SetValue(ref _DiamondProduct, value); }

        #endregion Properties

        #region Commands

        public ICommand VIPClickCommand { get => new RelayCommand(VIPClick); }

        private async void VIPClick()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var billingResult = await _inAppBillingService.PurchaseAsync(VIPProduct.ProductId, ItemType.Subscription);
            if (billingResult.PurchaseStateType == PosePurchaseStateType.Purchased)
            {
                ClientContext.SetCredentialsFrom(billingResult.PoseToken);
                _membershipService.SetMemberRoleType(billingResult.MemberRoleType);
                _membershipService.SetRoleExpireTime(billingResult.RoleExpireTime);
            }
            else
            {
                BillingErrorProcess(billingResult.PurchaseErrorType);
            }

            SetIsBusy(false);
        }

        public ICommand DiamondClickCommand { get => new RelayCommand(DiamondClick); }

        private async void DiamondClick()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var billingResult = await _inAppBillingService.PurchaseAsync(DiamondProduct.ProductId, ItemType.InAppPurchase);
            if (billingResult.PurchaseStateType == PosePurchaseStateType.Purchased)
            {
                ClientContext.SetCredentialsFrom(billingResult.PoseToken);
                _membershipService.SetMemberRoleType(billingResult.MemberRoleType);
                _membershipService.SetRoleExpireTime(billingResult.RoleExpireTime);
            }
            else
            {
                BillingErrorProcess(billingResult.PurchaseErrorType);
            }

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public VIPClubViewModel(
            VipClubPage page,
            InAppBillingService inAppBillingService,
            MembershipService membershipService) : base(page)
        {
            _inAppBillingService = inAppBillingService;
            _membershipService = membershipService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        private async void BillingErrorProcess(PosePurchaseErrorType errorType)
        {
            switch (errorType)
            {
                case PosePurchaseErrorType.AlreadyOwned:
                    {
                        await MaterialDialog.Instance.AlertAsync(LocalizeString.Already_Owned_Purchase,
                            LocalizeString.App_Title,
                            LocalizeString.Ok,
                            DialogConfiguration.DefaultAlterDialogConfiguration);
                    }
                    break;

                case PosePurchaseErrorType.FailStoreConnect:
                    {
                        await MaterialDialog.Instance.AlertAsync(LocalizeString.Fail_Store_Connect,
                            LocalizeString.App_Title,
                            LocalizeString.Ok,
                            DialogConfiguration.DefaultAlterDialogConfiguration);
                    }
                    break;

                case PosePurchaseErrorType.SuccessPurchaseButServerError: // 복구 필요
                    {
                        await MaterialDialog.Instance.AlertAsync(LocalizeString.Success_Purchase_But_Server_Error,
                            LocalizeString.App_Title,
                            LocalizeString.Restore,
                            DialogConfiguration.DefaultAlterDialogConfiguration);

                        while (!await _inAppBillingService.RestorePurchasedItem())
                        {
                            await MaterialDialog.Instance.AlertAsync(LocalizeString.Product_Not_Consumed,
                                LocalizeString.App_Title,
                                LocalizeString.Ok,
                                DialogConfiguration.DefaultAlterDialogConfiguration);
                        }
                    }
                    break;

                case PosePurchaseErrorType.UnknownError:
                    await MaterialDialog.Instance.AlertAsync(LocalizeString.Unknown_Error,
                           LocalizeString.App_Title,
                           LocalizeString.Restore,
                           DialogConfiguration.DefaultAlterDialogConfiguration);
                    break;

                default:
                    break;
            }
        }

        #endregion Methods
    }
}