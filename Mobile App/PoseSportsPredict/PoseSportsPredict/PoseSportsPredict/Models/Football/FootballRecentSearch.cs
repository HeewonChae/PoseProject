using PoseSportsPredict.InfraStructure.SQLite;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoseSportsPredict.Models.Football
{
    public class FootballRecentSearch : ISQLiteStorable
    {
        #region ISQLiteStorable

        public string _primaryKey;

        [PrimaryKey]
        public string PrimaryKey { get => $"{Keyword}"; set => _primaryKey = value; }

        public int Order { get; set; }
        public DateTime StoredTime { get; set; }

        #endregion ISQLiteStorable

        public string Keyword { get; set; }
        public string Logo { get; set; }
    }
}