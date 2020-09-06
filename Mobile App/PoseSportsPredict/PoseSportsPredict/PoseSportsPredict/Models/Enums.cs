namespace PoseSportsPredict.Models.Enums
{
    public enum MatchFilterType
    {
        Bookmark,
        Ongoing,
        SortByTime,
        SortByLeague,
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