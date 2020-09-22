using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Enum;
using PoseSportsPredict.Logics;
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
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class MyProfileViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            MessagingCenter.Subscribe<MembershipService, MemberRoleType>(this, AppConfig.MEMBERSHIP_TYPE_CHANGED, (s, e) => MembershipTypeChanged(e));

            return true;
        }

        public override Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            MembershipTypeChanged(_membershipService.MemberRoleType);
            return base.OnPrepareViewAsync(datas);
        }

        #endregion NavigableViewModel

        #region Services

        private IOAuthService _OAuthService;
        private MembershipService _membershipService;

        #endregion Services

        #region Fields

        private ExternAuthUser _authUser;
        private long _userNo;
        private MemberRoleType _memberRoleType;
        private DateTime _roleExpireTime;
        private bool _isValidExpireTime;
        private string _logonMessage;

        #endregion Fields

        #region Properties

        public ExternAuthUser AuthUser { get => _authUser; set => SetValue(ref _authUser, value); }
        public long UserNo { get => _userNo; set => SetValue(ref _userNo, value); }
        public MemberRoleType MemberRoleType { get => _memberRoleType; set => SetValue(ref _memberRoleType, value); }
        public DateTime RoleExpireTime { get => _roleExpireTime; set => SetValue(ref _roleExpireTime, value); }
        public bool IsValidExpireTime { get => _isValidExpireTime; set => SetValue(ref _isValidExpireTime, value); }
        public string LogonMessage { get => _logonMessage; set => SetValue(ref _logonMessage, value); }

        #endregion Properties

        #region Commands

        public ICommand LogonCommand { get => new RelayCommand(Logon); }

        private async void Logon()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            if (_OAuthService.IsAuthenticated)
            {
                bool? isLogout = await MaterialDialog.Instance.ConfirmAsync(
                LocalizeString.Want_Logout,
                LocalizeString.App_Title,
                LocalizeString.Ok,
                LocalizeString.Cancel,
                DialogConfiguration.AppTitleAlterDialogConfiguration);

                if (isLogout.HasValue && isLogout.Value)
                {
                    await _OAuthService.Logout();
                    await LoginFacade.GuestLogin();
                }
            }
            else
            {
                await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<LoginViewModel>());
            }

            SetIsBusy(false);
        }

        #endregion Commands

        #region Constructors

        public MyProfileViewModel(
            MyProfilePage page,
            IOAuthService oAuthService,
            MembershipService membershipService) : base(page)
        {
            _OAuthService = oAuthService;
            _membershipService = membershipService;
            OnInitializeView();
        }

        #endregion Constructors

        #region Methods

        private void MembershipTypeChanged(MemberRoleType value)
        {
            AuthUser = _OAuthService.AuthenticatedUser ?? new ExternAuthUser
            {
                FirstName = "Guest",
                PictureUrl = "ic_profile.png",
            };

            UserNo = ClientContext.UserNo;
            MemberRoleType = _membershipService.MemberRoleType;
            RoleExpireTime = _membershipService.RoleExpireTime.ToLocalTime();
            IsValidExpireTime = RoleExpireTime > DateTime.Now;
            LogonMessage = _OAuthService.IsAuthenticated ? LocalizeString.Logout : LocalizeString.Login;
        }

        #endregion Methods
    }
}