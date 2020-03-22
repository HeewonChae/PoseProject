﻿using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure.SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballMatchInfo : ISQLiteStorable, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        public int Id { get; set; }
        public string CountryName { get; set; }
        public string CountryLogo { get; set; }
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public string HomeName { get; set; }
        public string HomeLogo { get; set; }
        public short HomeScore { get; set; }
        public string AwayName { get; set; }
        public string AwayLogo { get; set; }
        public short AwayScore { get; set; }
        public FootballMatchStatusType MatchStatus { get; set; }
        public FootballLeagueType LeagueType { get; set; }
        public DateTime MatchTime { get; set; }
        public bool IsBookmarked { get; set; }
        public bool IsAlarmed { get; set; }

        public string GetPrimaryKey()
        {
            return this.Id.ToString();
        }
    }
}