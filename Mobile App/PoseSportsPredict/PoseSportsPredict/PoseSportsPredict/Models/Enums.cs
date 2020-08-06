namespace PoseSportsPredict.Models.Enums
{
    public enum SportsType
    {
        Football,
    }

    public enum TeamCampType
    {
        Any,
        Home,
        Away,
    }

    public enum MatchFilterType
    {
        Bookmark,
        Ongoing,
        SortByTime,
        SortByLeague,
    }

    public enum BookMarkType
    {
        Match,
        League,
        Team,
    }

    public enum NotificationType
    {
        MatchStart
    }

    public enum MatchResultType
    {
        Win,
        Draw,
        Lose,
    }

    public enum StandingsDescCategoryType
    {
        None,
        Positive,
        Neutral,
        Negative,
    }
}