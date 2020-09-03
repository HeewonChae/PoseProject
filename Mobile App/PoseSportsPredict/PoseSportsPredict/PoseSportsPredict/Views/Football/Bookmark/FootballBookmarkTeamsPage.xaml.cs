using MarcTron.Plugin.Controls;
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

namespace PoseSportsPredict.Views.Football.Bookmark
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FootballBookmarkTeamsPage : ContentPage
    {
        public FootballBookmarkTeamsPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<MembershipService, MemberRoleType>(this, AppConfig.MEMBERSHIP_TYPE_CHANGED, (s, e) => MembershipTypeChanged(e));
        }

        private MemberRoleType oldMemberRoleType;

        private void MTAdView_AdsLoaded(object sender, EventArgs e)
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