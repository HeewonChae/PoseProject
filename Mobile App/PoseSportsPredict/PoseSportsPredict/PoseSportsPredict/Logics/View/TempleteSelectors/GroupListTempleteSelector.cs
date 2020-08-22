using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.TempleteSelectors
{
    public class GroupListTempleteSelector : DataTemplateSelector
    {
        public DataTemplate CollapsedTemplate { get; set; }
        public DataTemplate ExpandedTamplete { get; set; }
        public DataTemplate AdsTamplete { get; set; }
        public DataTemplate RecommendTamplete { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is IExpandable group))
                return null;

            DataTemplate result = null;
            switch (group.GroupType)
            {
                case Models.Enums.MatchGroupType.Default:
                    if (group.Expanded)
                        result = ExpandedTamplete;
                    else
                        result = CollapsedTemplate;
                    break;

                case Models.Enums.MatchGroupType.Recommand:
                    result = RecommendTamplete;
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}