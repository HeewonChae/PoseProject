using PoseSportsPredict.InfraStructure.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballTeamInfo : ISQLiteStorable, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region ISQLiteStorable

        public string _primaryKey;

        [PrimaryKey]
        public string PrimaryKey { get => $"{CountryName}:{LeagueName}:{TeamName}"; set => _primaryKey = value; }

        public int Order { get; set; }
        public DateTime StoredTime { get; set; }

        #endregion ISQLiteStorable

        public short TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamLogo { get; set; }
        public string LeagueName { get; set; }
        public string LeagueLogo { get; set; }
        public string CountryName { get; set; }
        public string CountryLogo { get; set; }
        public bool IsBookmarked { get; set; }
    }
}