using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballLeaguesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override void OnAppearing(params object[] datas)
        {
        }

        #endregion NavigableViewModel

        #region Constructors

        public FootballLeaguesViewModel(FootballLeaguesPage page) : base(page)
        {
            OnInitializeView();
        }

        #endregion Constructors
    }
}