using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.Logics.View.Triggers
{
    public class SelectedTeamLabelTrigger_Away : TriggerAction<XF.Material.Forms.UI.MaterialLabel>
    {
        protected override void Invoke(MaterialLabel sender)
        {
            short.TryParse(sender.Text, out short teamdId);

            if (sender.BindingContext is FootballMatchInfo matchInfo)
            {
                sender.Text = matchInfo.AwayName;
                sender.FontAttributes = matchInfo.AwayTeamId == teamdId ? FontAttributes.Bold : FontAttributes.None;
            }
        }
    }
}