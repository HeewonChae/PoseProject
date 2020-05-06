using PoseSportsPredict.Models.Football;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.Logics.View.Triggers
{
    public class SelectedTeamLabelTrigger_Home : TriggerAction<XF.Material.Forms.UI.MaterialLabel>
    {
        protected override void Invoke(MaterialLabel sender)
        {
            short.TryParse(sender.Text, out short teamdId);

            if (sender.BindingContext is FootballMatchInfo matchInfo)
            {
                sender.Text = matchInfo.HomeName;
                sender.FontAttributes = matchInfo.HomeTeamId == teamdId ? FontAttributes.Bold : FontAttributes.None;
            }
        }
    }
}