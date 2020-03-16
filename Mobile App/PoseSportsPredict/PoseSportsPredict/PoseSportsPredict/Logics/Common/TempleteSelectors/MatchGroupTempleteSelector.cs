using PoseSportsPredict.Models;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.Common.TempleteSelectors
{
    public class MatchGroupTempleteSelector : DataTemplateSelector
    {
        public DataTemplate CollapsedTemplate { get; set; }
        public DataTemplate ExpandedTamplete { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var matchGroup = item as MatchGroup;
            if (matchGroup == null)
                return null;

            if (matchGroup.Expanded)
                return ExpandedTamplete;
            else
                return CollapsedTemplate;
        }
    }
}