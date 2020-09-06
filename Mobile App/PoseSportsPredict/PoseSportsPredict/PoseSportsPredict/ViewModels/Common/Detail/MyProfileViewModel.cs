using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Auth.Models;
using PosePacket.Service.Enum;
using PoseSportsPredict.Resources;
using PoseSportsPredict.Services;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Common.Detail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WebServiceShare.ExternAuthentication;
using WebServiceShare.ServiceContext;
using XF.Material.Forms.UI.Dialogs;

namespace PoseSportsPredict.ViewModels.Common.Detail
{
    public class MyProfileViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override Task<bool> OnPrepareViewAsync(params object[] datas)
        {
            AuthUser = _OAuthService.AuthenticatedUser;
            UserNo = ClientContext.UserNo;
            MemberRoleType = _membershipService.MemberRoleType;
            RoleExpireTime = _membershipService.RoleExpireTime.ToLocalTime();
            IsValidExpireTime = RoleExpireTime > DateTime.Now;

            return Task.FromResult(true);
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

        #endregion Fields

        #region Properties

        public ExternAuthUser AuthUser { get => _authUser; set => SetValue(ref _authUser, value); }
        public long UserNo { get => _userNo; set => SetValue(ref _userNo, value); }
        public MemberRoleType MemberRoleType { get => _memberRoleType; set => SetValue(ref _memberRoleType, value); }
        public DateTime RoleExpireTime { get => _roleExpireTime; set => SetValue(ref _roleExpireTime, value); }
        public bool IsValidExpireTime { get => _isValidExpireTime; set => SetValue(ref _isValidExpireTime, value); }

        #endregion Properties

        #region Commands

        public ICommand LogoutCommand { get => new RelayCommand(Logout); }

        private async void Logout()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            bool? isLogout = await MaterialDialog.Instance.ConfirmAsync(
                LocalizeString.Do_You_Want_Logout,
                LocalizeString.App_Title,
                LocalizeString.Ok,
                LocalizeString.Cancel,
                DialogConfiguration.AppTitleAlterDialogConfiguration);

            if (isLogout.HasValue && isLogout.Value)
                await _OAuthService.Logout();

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
        }

        #endregion Constructors
    }
}