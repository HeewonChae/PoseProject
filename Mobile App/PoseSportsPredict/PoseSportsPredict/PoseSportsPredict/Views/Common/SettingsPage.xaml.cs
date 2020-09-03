using PosePacket.Service.Enum;
using PoseSportsPredict.Models.Resources.Common;
using PoseSportsPredict.Services;
using Shiny;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PoseSportsPredict.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private MemberRoleType oldMemberRoleType;

        public SettingsPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<MembershipService, MemberRoleType>(this, AppConfig.MEMBERSHIP_TYPE_CHANGED, (s, e) => MembershipTypeChanged(e));
        }

        private void _AdMob_AdsLoaded(object sender, EventArgs e)
        {
            if (MembershipAdvantage.TryGetValue(oldMemberRoleType, out MembershipAdvantage advantage))
            {
                _AdMob.IsVisible = !advantage.IsBannerAdRemove;
            }
        }

        private void MembershipTypeChanged(MemberRoleType value)
        {
            if (oldMemberRoleType != value)
            {
                if (MembershipAdvantage.TryGetValue(value, out MembershipAdvantage advantage))
                {
                    if (_AdMob.IsVisible)
                    {
                        _AdMob.IsVisible = !advantage.IsBannerAdRemove;
                    }
                }

                oldMemberRoleType = value;
            }
        }
    }
}