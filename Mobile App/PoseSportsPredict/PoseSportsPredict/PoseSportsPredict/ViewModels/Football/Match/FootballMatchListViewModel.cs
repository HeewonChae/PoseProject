using GalaSoft.MvvmLight.Command;
using PosePacket.Service.Football;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.ViewModels.Base;
using Shiny;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

using PacketModels = PosePacket.Service.Football.Models;

namespace PoseSportsPredict.ViewModels.Football
{
    public class FootballMatchListViewModel : BaseViewModel
    {
        #region Properties

        public int MatchCount => Matches?.Count ?? 0;
        public ObservableCollection<PacketModels.FixtureDetail> Matches { get; set; }

        #endregion Properties

        #region Commands

        public ICommand SelectMatchCommand { get => new RelayCommand<PacketModels.FixtureDetail>((e) => SelectMatch(e)); }

        private async void SelectMatch(PacketModels.FixtureDetail matchInfo)
        {
            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>(), matchInfo);
        }

        #endregion Commands
    }
}