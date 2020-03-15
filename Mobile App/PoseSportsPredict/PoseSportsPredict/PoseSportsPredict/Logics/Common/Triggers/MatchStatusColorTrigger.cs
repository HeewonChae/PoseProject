using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace PoseSportsPredict.Logics.Common.Triggers
{
    internal class MatchStatusColorTrigger : TriggerAction<XF.Material.Forms.UI.MaterialChip>
    {
        protected override void Invoke(MaterialChip sender)
        {
            var textValue = sender.Text;

            if (textValue.Equals("FT")
                || textValue.Equals("AET")
                || textValue.Equals("PEN"))
            {
                sender.BackgroundColor = AppResourcesHelper.GetResourceColor("CustomGrey");
            }
            else if (textValue.Equals("NS"))
            {
                sender.BackgroundColor = AppResourcesHelper.GetResourceColor("PrimaryColor_D");
            }
            else
            {
                sender.BackgroundColor = AppResourcesHelper.GetResourceColor("PrimaryColor_L");
            }
        }
    }
}