using GalaSoft.MvvmLight.Command;
using Plugin.InAppBilling.Abstractions;
using PosePacket.Service.Billing.Models.Enums;
using PosePacket.Service.Enum;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common.Detail;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ServiceContext;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class VIPSubscribeViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            _VIPProduct = _inAppBillingService?.SubscriptionProduct?.FirstOrDefault() ?? null;
            _DiamondProduct = _inAppBillingService?.InAppProduct?.FirstOrDefault() ?? null;

            var screenHelper = DependencyService.Resolve<IScreenHelper>();
            CardMaxWidth = screenHelper.ScreenSize.Width - 33;
            return true;
        }

        public override void OnAppearing(params object[] datas)
        {
            RefrashPurchase();
        }

        #endregion NavigableViewModel

        #region Service

        private InAppBillingService _inAppBillingService;
        private MembershipService _membershipService;

        #endregion Service

        #region Fields

        public double CardMaxWidth;
        private InAppBillingProduct _VIPProduct;
        private InAppBillingProduct _DiamondProduct;
        private IEnumerable<InAppPurchaseInfo> _inAppPurchases;

        #endregion Fields

        #region Properties

        public IEnumerable<InAppPurchaseInfo> InAppPurchases { get => _inAppPurchases; set => SetValue(ref _inAppPurchases, value); }

        #endregion Properties

        #region Commands

        public ICommand PurchaseButtonClickCommand { get => new RelayCommand<InAppPurchaseInfo>(e => PurchaseButtonClick(e)); }

        private async void PurchaseButtonClick(InAppPurchaseInfo purchaseItem)
        {
            if (IsBusy || purchaseItem.IsActivated)
                return;

            SetIsBusy(true);

            var billingResult = await _inAppBillingService.PurchaseAsync(purchaseItem.ProductId, purchaseItem.PurchaseType);
            if (billingResult.PurchaseStateType == PosePurchaseStateType.Purchased)
            {
                ClientContext.SetCredentialsFrom(billingResult.PoseToken);
                _membershipService.SetMemberRoleType(billingResult.MemberRoleType);
                _membershipService.SetRoleExpireTime(billingResult.RoleExpireTime);
                RefrashPurchase();
            }
            else
            {
                BillingErrorProcess(billingResult.PurchaseErrorType);
            }

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public VIPSubscribeViewModel(
            VIPSubscribePage page,
            InAppBillingService inAppBillingService,
            MembershipService membershipService) : base(page)
        {
            _inAppBillingService = inAppBillingService;
            _membershipService = membershipService;

            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        private void RefrashPurchase()
        {
            // 다이아몬드
            var diamond = new InAppPurchaseInfo();
            diamond.ProductId = _DiamondProduct?.ProductId ?? string.Empty;
            diamond.PurchaseType = ItemType.InAppPurchase;
            diamond.Title = LocalizeString.Diamond;
            diamond.TitleColor = Color.DarkTurquoise;
            diamond.ImageResource = "ic_diamond.png";
            diamond.ImageColor = Color.DarkTurquoise;
            diamond.ButtonText = _membershipService.MemberRoleType > MemberRoleType.Regular ? LocalizeString.Applying : $"{_DiamondProduct?.LocalizedPrice}";
            diamond.IsActivated = _membershipService.MemberRoleType > MemberRoleType.Regular;
            diamond.IsAvailable = _DiamondProduct != null;
            diamond.Advantages = new string[] {
                $"● {LocalizeString.Diamond_Advantage_1}",
                $"● {LocalizeString.Provide_VIP_Picks}",
                $"● {LocalizeString.Diamond_Advantage_2}",
                $"● {LocalizeString.Diamond_Advantage_3}",
                $"● {LocalizeString.Diamond_Advantage_4}",
                ""};

            // VIP
            var vip = new InAppPurchaseInfo();
            vip.ProductId = _VIPProduct?.ProductId ?? string.Empty;
            vip.PurchaseType = ItemType.Subscription;
            vip.Title = LocalizeString.VIP;
            vip.TitleColor = Color.Gold;
            vip.ImageResource = "ic_vip.png";
            vip.ImageColor = Color.Gold;
            vip.ButtonText = _membershipService.MemberRoleType > MemberRoleType.Diamond ? LocalizeString.Applying : $"{_VIPProduct?.LocalizedPrice}/{LocalizeString.Monthly}";
            vip.IsActivated = _membershipService.MemberRoleType > MemberRoleType.Diamond;
            vip.IsAvailable = _VIPProduct != null;
            vip.Advantages = new string[] {
                $"● {LocalizeString.VIP_Advantage_1}",
                $"● {LocalizeString.Provide_VIP_Picks}",
                $"● {LocalizeString.VIP_Advantage_2}",
                $"● {LocalizeString.VIP_Advantage_3}",
                $"● {LocalizeString.VIP_Advantage_4}",
                $"● {LocalizeString.VIP_Advantage_5}" };

            InAppPurchases = new InAppPurchaseInfo[] { vip, diamond };
        }

        private async void BillingErrorProcess(PosePurchaseErrorType errorType)
        {
            switch (errorType)
            {
                case PosePurchaseErrorType.AlreadyOwned:
                    {
                        await MaterialDialog.Instance.AlertAsync(LocalizeString.Already_Owned_Purchase,
                            LocalizeString.App_Title,
                            LocalizeString.Ok,
                            DialogConfiguration.AppTitleAlterDialogConfiguration);
                    }
                    break;

                case PosePurchaseErrorType.FailStoreConnect:
                    {
                        await MaterialDialog.Instance.AlertAsync(LocalizeString.Fail_Store_Connect,
                            LocalizeString.App_Title,
                            LocalizeString.Ok,
                            DialogConfiguration.AppTitleAlterDialogConfiguration);
                    }
                    break;

                case PosePurchaseErrorType.SuccessPurchaseButServerError: // 복구 필요
                    {
                        await MaterialDialog.Instance.AlertAsync(LocalizeString.Success_Purchase_But_Server_Error,
                            LocalizeString.App_Title,
                            LocalizeString.Restore,
                            DialogConfiguration.AppTitleAlterDialogConfiguration);

                        while (!await _inAppBillingService.RestorePurchasedItem())
                        {
                            await MaterialDialog.Instance.AlertAsync(LocalizeString.Product_Not_Consumed,
                                LocalizeString.App_Title,
                                LocalizeString.Ok,
                                DialogConfiguration.AppTitleAlterDialogConfiguration);
                        }
                    }
                    break;

                case PosePurchaseErrorType.UnknownError:
                    await MaterialDialog.Instance.AlertAsync(LocalizeString.Unknown_Error,
                           LocalizeString.App_Title,
                           LocalizeString.Restore,
                           DialogConfiguration.AppTitleAlterDialogConfiguration);
                    break;

                default:
                    break;
            }
        }

        #endregion Methods
    }
}