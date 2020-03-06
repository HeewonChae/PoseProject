using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.Views.Football;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballLeaguesViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override async Task<bool> PrepareView(params object[] data)
        {
            return true;
        }

        #endregion BaseViewModel

        #region Constructors

        public FootballLeaguesViewModel(FootballLeaguesPage page) : base(page)
        {
        }

        #endregion Constructors
    }
}