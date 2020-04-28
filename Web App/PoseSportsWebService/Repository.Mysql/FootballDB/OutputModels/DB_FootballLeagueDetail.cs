using Repository.Mysql.FootballDB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.OutputModels
{
    public class DB_FootballLeagueDetail
    {
        public string CountryName { get; set; }
        public string CountryLogo { get; set; }
        public short Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string LeagueType { get; set; }
        public DateTime SeasonStartDate { get; set; }
        public DateTime SeasonEndDate { get; set; }

        #region Select Query

        private static string _selectQuery;

        public static string SelectQuery
        {
            get
            {
                if (string.IsNullOrEmpty(_selectQuery))
                {
                    var sb = new StringBuilder();
                    sb.Append($"SELECT l.{nameof(League.id)} as {nameof(Id)}, l.{nameof(League.name)} as {nameof(Name)}, l.{nameof(League.logo)} as {nameof(Logo)}, l.{nameof(League.type)} as {nameof(LeagueType)}, ");
                    sb.Append($"l.{nameof(League.season_start)} as {nameof(SeasonStartDate)}, l.{nameof(League.season_end)} as {nameof(SeasonEndDate)}, ");
                    sb.Append($"c.{nameof(Country.name)} as {nameof(CountryName)}, c.{nameof(Country.logo)} as {nameof(CountryLogo)} ");
                    sb.Append("FROM league as l ");
                    sb.Append($"INNER JOIN country as c on l.{nameof(League.country_name)} = c.{nameof(Country.name)} ");

                    _selectQuery = sb.ToString();
                }

                return _selectQuery;
            }
        }

        #endregion Select Query
    }
}