using PosePacket.Service.Football.Models.Enums;
using PoseSportsPredict.InfraStructure;
using PoseSportsPredict.InfraStructure.SQLite;
using PoseSportsPredict.Models.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballLeagueInfo : IBookmarkMenuItem, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region IBookmarkMenuItem

        public string _primaryKey;

        [PrimaryKey]
        public string PrimaryKey { get => $"{CountryName}:{LeagueName}"; set => _primaryKey = value; }

        public int Order { get; set; }
        public DateTime StoredTime { get; set; }
        public string MenuName { get => LeagueName; }
        public string Logo { get => LeagueLogo; }
        public BookMarkType BookMarkType { get => BookMarkType.League; }

        #endregion IBookmarkMenuItem

        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public FootballLeagueType LeagueType { get; set; }
        public string CountryName { get; set; }
        public string CountryLogo { get; set; }
        public DateTime SeasonStartDate { get; set; }
        public DateTime SeasonEndDate { get; set; }
        public bool IsBookmarked { get; set; }
    }
}