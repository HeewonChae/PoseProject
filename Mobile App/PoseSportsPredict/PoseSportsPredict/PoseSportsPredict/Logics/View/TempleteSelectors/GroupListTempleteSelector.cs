﻿using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Models;
using Xamarin.Forms;

namespace PoseSportsPredict.Logics.View.TempleteSelectors
{
    public class GroupListTempleteSelector : DataTemplateSelector
    {
        public DataTemplate CollapsedTemplate { get; set; }
        public DataTemplate ExpandedTamplete { get; set; }
        public DataTemplate MediumAdsTamplete { get; set; }
        public DataTemplate MediumAds2Tamplete { get; set; }
        public DataTemplate SmallAdsTamplete { get; set; }
        public DataTemplate RecommendTamplete { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is ITempletable group))
                return null;

            DataTemplate result = null;
            switch (group.GroupType)
            {
                case Models.Enums.MatchGroupType.Default:
                    if (group is IExpandable expand)
                        result = expand.Expanded ? ExpandedTamplete : CollapsedTemplate;
                    break;

                case Models.Enums.MatchGroupType.Recommand:
                    result = RecommendTamplete;
                    break;

                case Models.Enums.MatchGroupType.NativeAds:
                    if (group.AdsBannerType == Models.Enums.AdsBannerType.NativeMedium)
                        result = MediumAdsTamplete;
                    else if (group.AdsBannerType == Models.Enums.AdsBannerType.NativeMedium2)
                        result = MediumAds2Tamplete;
                    else if (group.AdsBannerType == Models.Enums.AdsBannerType.NativeSmall)
                        result = SmallAdsTamplete;
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}