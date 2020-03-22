﻿using GalaSoft.MvvmLight.Command;
using PoseSportsPredict.Logics.Common;
using PoseSportsPredict.Logics.Football.Converters;
using PoseSportsPredict.Models.Football;
using PoseSportsPredict.ViewModels.Base;
using PoseSportsPredict.ViewModels.Football.Match.Detail;
using Shiny;
using System.Collections.ObjectModel;
using System.Windows.Input;

using PacketModels = PosePacket.Service.Football.Models;

namespace PoseSportsPredict.ViewModels.Football.Match
{
    public class FootballMatchListViewModel : BaseViewModel
    {
        #region Properties

        public int MatchCount => Matches?.Count ?? 0;
        public ObservableCollection<PacketModels.FixtureDetail> Matches { get; set; }

        #endregion Properties

        #region Commands

        public ICommand SelectMatchCommand { get => new RelayCommand<PacketModels.FixtureDetail>((e) => SelectMatch(e)); }

        private async void SelectMatch(PacketModels.FixtureDetail fixtureDetail)
        {
            if (IsBusy)
                return;

            SetIsBusy(true);

            await PageSwitcher.PushModalPageAsync(ShinyHost.Resolve<FootballMatchDetailViewModel>()
                , ShinyHost.Resolve<FixtureDetailToMatchInfoConverter>().Convert(fixtureDetail, null, null, null));

            SetIsBusy(false);
        }

        #endregion Commands
    }
}