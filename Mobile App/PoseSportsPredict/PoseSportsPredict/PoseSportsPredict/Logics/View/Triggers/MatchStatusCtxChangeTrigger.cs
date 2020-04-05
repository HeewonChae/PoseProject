using PosePacket.Service.Football.Models;
using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Models.Football;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.Logics.View.Triggers
{
    internal class MatchStatusCtxChangeTrigger : TriggerAction<XF.Material.Forms.UI.MaterialChip>
    {
        protected override void Invoke(MaterialChip sender)
        {
            if (sender.BindingContext is FootballFixtureDetail fixtureDetail)
            {
                if (fixtureDetail.MatchStatus == FootballMatchStatusType.FT
                || fixtureDetail.MatchStatus == FootballMatchStatusType.AET
                || fixtureDetail.MatchStatus == FootballMatchStatusType.PEN)
                {
                    sender.Text = $"{fixtureDetail.HomeTeam.Score} - {fixtureDetail.AwayTeam.Score}";
                    sender.BackgroundColor = AppResourcesHelper.GetResourceColor("CustomGrey_D");
                }
                else
                {
                    if (fixtureDetail.MatchStatus == FootballMatchStatusType.NS)
                    {
                        sender.Text = fixtureDetail.MatchStatus.ToString();
                        sender.BackgroundColor = AppResourcesHelper.GetResourceColor("PrimaryColor_D");
                    }
                    else
                    {
                        sender.Text = $"{fixtureDetail.HomeTeam.Score} - {fixtureDetail.AwayTeam.Score}";
                        sender.BackgroundColor = AppResourcesHelper.GetResourceColor("PrimaryColor_L");
                    }
                }
            }
            else if (sender.BindingContext is FootballMatchInfo matchInfo)
            {
                if (matchInfo.MatchStatus == FootballMatchStatusType.FT
                || matchInfo.MatchStatus == FootballMatchStatusType.AET
                || matchInfo.MatchStatus == FootballMatchStatusType.PEN)
                {
                    sender.Text = $"{matchInfo.HomeScore} - {matchInfo.AwayScore}";
                    sender.BackgroundColor = AppResourcesHelper.GetResourceColor("CustomGrey_D");
                }
                else
                {
                    if (matchInfo.MatchStatus == FootballMatchStatusType.NS)
                    {
                        sender.Text = matchInfo.MatchStatus.ToString();
                        sender.BackgroundColor = AppResourcesHelper.GetResourceColor("PrimaryColor_D");
                    }
                    else
                    {
                        sender.Text = $"{matchInfo.HomeScore} - {matchInfo.AwayScore}";
                        sender.BackgroundColor = AppResourcesHelper.GetResourceColor("PrimaryColor_L");
                    }
                }
            }
        }
    }
}