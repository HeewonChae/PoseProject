using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PoseSportsPredict.ViewModels.Football.Standings
{
    public class FootballStandingsViewModel : BaseViewModel
    {
        #region BaseViewModel

        public override bool OnInitializeView(params object[] datas)
        {
            if (datas is FootballStandingsInfo[] footballStandingsInfos)
            {
                StandingsInfos = new ObservableCollection<FootballStandingsInfo>(
                    footballStandingsInfos.OrderBy(elem => elem.Rank));
            }

            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private bool _isViewForm;
        private ObservableCollection<FootballStandingsInfo> _standingsInfos;

        #endregion Fields

        #region Properties

        public bool IsViewForm { get => _isViewForm; set => SetValue(ref _isViewForm, value); }
        public ObservableCollection<FootballStandingsInfo> StandingsInfos { get => _standingsInfos; set => SetValue(ref _standingsInfos, value); }

        #endregion Properties
    }
}