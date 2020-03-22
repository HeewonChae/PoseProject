﻿using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure.SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballLeagueInfo : ISQLiteStorable, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public FootballLeagueType LeagueType { get; set; }
        public string CountryName { get; set; }
        public string CountryLogo { get; set; }
        public bool IsBookmarked { get; set; }

        public string GetPrimaryKey()
        {
            return $"{CountryName}:{LeagueName}:{LeagueType}";
        }
    }
}