using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.TempleteSelectors
{
    public class GroupListTempleteSelector : DataTemplateSelector
    {
        public DataTemplate CollapsedTemplate { get; set; }
        public DataTemplate ExpandedTamplete { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is IGroupBase group))
                return null;

            if (group.Expanded)
                return ExpandedTamplete;
            else
                return CollapsedTemplate;
        }
    }
}