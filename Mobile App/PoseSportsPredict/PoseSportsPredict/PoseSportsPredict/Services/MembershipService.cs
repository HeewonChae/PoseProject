using PosePacket.Proxy;
using PosePacket.Service.Billing;
using PosePacket.Service.Billing.Models.Enums;
using PosePacket.Service.Enum;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Utilities;
using Shiny;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WebServiceShare.ServiceContext;
using WebServiceShare.WebServiceClient;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.Services
{
    public class MembershipService
    {
        private Timer _timer;
        private SecureString _memberRoleType;
        private DateTime _roleExpireTime;

        public MemberRoleType MemberRoleType => GetMemberRoleType();
        public DateTime RoleExpireTime => _roleExpireTime;

        public MemberRoleType GetMemberRoleType()
        {
            IntPtr pSecureString = Marshal.SecureStringToGlobalAllocUnicode(_memberRoleType);
            string strSecureString = Marshal.PtrToStringUni(pSecureString);

            strSecureString.TryParseEnum(out MemberRoleType memberRoleType);
            return memberRoleType;
        }

        public void SetMemberRoleType(MemberRoleType value)
        {
            string roleTypeToString = value.ToString();
            _memberRoleType = new SecureString();

            foreach (var elem in roleTypeToString)
                _memberRoleType.AppendChar(elem);

            // Send MembershipType changed message
            MessagingCenter.Send<MembershipService, MemberRoleType>(this, AppConfig.MEMBERSHIP_TYPE_CHANGED, value);
        }

        private const long THREE_HOUR_MILLI_SEC = 3 * 3600 * 1000;

        public void SetRoleExpireTime(DateTime value)
        {
            _timer?.Stop();
            _roleExpireTime = value;

            if (MemberRoleType == MemberRoleType.Promotion  // 프로모션 유저
                || MemberRoleType == MemberRoleType.Diamond // 결제 유저 등급
                || MemberRoleType == MemberRoleType.VIP)    // 결제 유저 등급
            {
                var expireMilliSec = (_roleExpireTime - DateTime.UtcNow).TotalMilliseconds;
                var timer_sec = expireMilliSec > THREE_HOUR_MILLI_SEC ? THREE_HOUR_MILLI_SEC : expireMilliSec;
                if (timer_sec > 0)
                {
                    _timer = new Timer(timer_sec);
                    _timer.Elapsed += OnCheckMemberRoleType;
                    _timer.AutoReset = false;
                    _timer.Enabled = true;
                }
                else
                    OnCheckMemberRoleType(null, null);
            }
        }

        private async void OnCheckMemberRoleType(Object source, ElapsedEventArgs e)
        {
            var webApiService = ShinyHost.Resolve<IWebApiService>();
            var deviceInfoHelper = DependencyService.Resolve<IDeviceInfoHelper>();
            var server_result = await webApiService.RequestAsyncWithToken<O_E_CHECK_MEMBERSHIP_BY_GOOGLE>(new WebRequestContext
            {
                SerializeType = SerializeType.MessagePack,
                MethodType = WebMethodType.POST,
                BaseUrl = AppConfig.PoseWebBaseUrl,
                ServiceUrl = BillingProxy.ServiceUrl,
                SegmentGroup = BillingProxy.P_E_CHECK_MEMBERSHIP_BY_GOOGLE,
                NeedEncrypt = true,
                PostData = new I_E_CHECK_MEMBERSHIP_BY_GOOGLE
                {
                    AppPackageName = deviceInfoHelper.AppPackageName,
                }
            });

            if (server_result != null)
            {
                ClientContext.SetCredentialsFrom(server_result.BillingResult.PoseToken);
                SetMemberRoleType(server_result.BillingResult.MemberRoleType);
                SetRoleExpireTime(server_result.BillingResult.RoleExpireTime);

                var purchaseState = server_result.BillingResult.PurchaseStateType;
                if (purchaseState == PosePurchaseStateType.Pending
                    || purchaseState == PosePurchaseStateType.Grace)
                {
                    var ret = await MaterialDialog.Instance.ConfirmAsync(LocalizeString.Payment_Pending_Please_Check,
                            LocalizeString.App_Title,
                            LocalizeString.Ok,
                            LocalizeString.Cancel,
                            DialogConfiguration.AppTitleAlterDialogConfiguration);

                    if (ret.HasValue && ret.Value)
                    {
                        using (Acr.UserDialogs.UserDialogs.Instance.Loading(LocalizeString.Loading))
                        {
                            await Browser.OpenAsync(
                            $"https://play.google.com/store/account/subscriptions?sku={server_result.BillingResult.ProductId}&package={deviceInfoHelper.AppPackageName}",
                            BrowserLaunchMode.SystemPreferred);
                        }
                    }
                }
            }
        }
    }
}