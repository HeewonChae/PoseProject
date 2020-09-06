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
    public class FootballTeamInfo : IBookmarkMenuItem, INotifyPropertyChanged
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
        public string PrimaryKey { get => $"{CountryName}:{TeamName}"; set => _primaryKey = value; }

        public int Order { get; set; }
        public DateTime StoredTime { get; set; }
        public string MenuName { get => TeamName; }
        public string Logo { get => TeamLogo; }
        public PageDetailType BookMarkType { get => PageDetailType.Team; }

        #endregion IBookmarkMenuItem

        public short TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public string CountryName { get; set; }
        public bool IsBookmarked { get; set; }
    }
}