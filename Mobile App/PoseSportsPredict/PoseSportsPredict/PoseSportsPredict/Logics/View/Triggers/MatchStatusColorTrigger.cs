using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.Logics.View.Triggers
{
    public class MatchStatusColorTrigger : TriggerAction<XF.Material.Forms.UI.MaterialChip>
    {
        protected override void Invoke(MaterialChip sender)
        {
            short.TryParse(sender.Text, out short teamdId);

            if (sender.BindingContext is FootballMatchInfo matchInfo)
            {
                if (matchInfo.MatchStatus == FootballMatchStatusType.FT
                || matchInfo.MatchStatus == FootballMatchStatusType.AET
                || matchInfo.MatchStatus == FootballMatchStatusType.PEN)
                {
                    sender.Text = $"{matchInfo.HomeScore} - {matchInfo.AwayScore}";

                    bool isDraw = matchInfo.HomeScore == matchInfo.AwayScore;
                    bool isWin = matchInfo.HomeTeamId == teamdId ? matchInfo.HomeScore > matchInfo.AwayScore : matchInfo.HomeScore < matchInfo.AwayScore;

                    sender.BackgroundColor =
                        isDraw ?
                        AppResourcesHelper.GetResourceColor("CustomGrey") :
                        isWin ? AppResourcesHelper.GetResourceColor("WinColor") : AppResourcesHelper.GetResourceColor("LoseColor");
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