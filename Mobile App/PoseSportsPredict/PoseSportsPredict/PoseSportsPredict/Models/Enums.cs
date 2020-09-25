namespace PoseSportsPredict.Models.Enums
{
    public enum MatchFilterType
    {
        AllMatches,
        Bookmark,
        Recommended,
        Ongoing,
        SortByTime,
    }

    public enum PageDetailType
    {
        Match,
        League,
        Team,
    }

    public enum NotificationType
    {
        MatchStart
    }

    public enum StandingsDescCategoryType
    {
        None,
        Positive,
        Neutral,
        Negative,
    }

    public enum MatchGroupType
    {
        Default,
        NativeAds,
        Recommand,
    }

    public enum AdsBannerType
    {
        NativeSmall,
        NativeMedium,
        NativeMedium2,
    }
}