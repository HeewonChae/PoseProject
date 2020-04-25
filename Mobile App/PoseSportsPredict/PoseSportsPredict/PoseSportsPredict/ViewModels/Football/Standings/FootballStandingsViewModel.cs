using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.Logics;
using PoseSportsPredict.Models.Enums;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.Models.Resources.Football;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.League.Detail;
using PoseSportsPredict.ViewModels.Football.Team;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

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

                LeagueTitle = footballStandingsInfos.First().Group;

                // Description Groupping
                var groupByDescription = footballStandingsInfos.GroupBy(elem => elem.Description);
                StandingsDescription = new ObservableCollection<FootballStandingsDescription>();

                foreach (var groupData in groupByDescription)
                {
                    if (string.IsNullOrEmpty(groupData.Key))
                        continue;

                    StandingsDescription.Add(new FootballStandingsDescription
                    {
                        Description = groupData.Key,
                        DescColor = groupData.First().RankColor,
                    });
                }
            }

            return true;
        }

        #endregion BaseViewModel

        #region Fields

        private bool _isViewForm;
        private string _leagueTitle;
        private ObservableCollection<FootballStandingsInfo> _standingsInfos;
        private ObservableCollection<FootballStandingsDescription> _standingsDescription;

        #endregion Fields

        #region Properties

        public bool IsViewForm { get => _isViewForm; set => SetValue(ref _isViewForm, value); }
        public string LeagueTitle { get => _leagueTitle; set => SetValue(ref _leagueTitle, value); }
        public ObservableCollection<FootballStandingsInfo> StandingsInfos { get => _standingsInfos; set => SetValue(ref _standingsInfos, value); }
        public ObservableCollection<FootballStandingsDescription> StandingsDescription { get => _standingsDescription; set => SetValue(ref _standingsDescription, value); }

        #endregion Properties

        #region Commands

        public ICommand LeagueNameClickCommand { get => new RelayCommand(LeagueNameClick); }

        private async void LeagueNameClick()
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            var standingsFirstData = StandingsInfos.First();
            FootballLeagueInfo leagueInfo = new FootballLeagueInfo
            {
                LeagueName = standingsFirstData.LeagueName,
                LeagueLogo = standingsFirstData.LeagueLogo,
                LeagueType = standingsFirstData.LeagueType,
                CountryName = standingsFirstData.CountryName,
                CountryLogo = standingsFirstData.CountryLogo,
            };

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballLeagueDetailViewModel>(), leagueInfo);

            SetIsBusy(false);
        }

        public ICommand TeamNameClickCommand { get => new RelayCommand<FootballStandingsInfo>(e => TeamNameClick(e)); }

        private async void TeamNameClick(FootballStandingsInfo standings)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            FootballTeamInfo teamInfo = new FootballTeamInfo
            {
                TeamId = standings.TeamId,
                TeamName = standings.TeamName,
                TeamLogo = standings.TeamLogo,
                CountryName = standings.CountryName,
                CountryLogo = standings.CountryLogo,
            };

            await PageSwitcher.PushNavPageAsync(ShinyHost.Resolve<FootballTeamDetailViewModel>(), teamInfo);

            SetIsBusy(false);
        }

        #endregion Commands
    }
}