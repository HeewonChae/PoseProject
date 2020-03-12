using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.Views.Football;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballLeaguesViewModel : NavigableViewModel
    {
        #region NavigableViewModel

        public override bool OnAppearing(params object[] datas)
        {
            return true;
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