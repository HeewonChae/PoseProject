using Repository.Mysql.FootballDB.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.OutputModels
{
    public class DB_FootballTeamDetail
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string CountryName { get; set; }
        public string CountryLogo { get; set; }

        #region Select Query

        private static string _selectQuery;

        public static string SelectQuery
        {
            get
            {
                if (string.IsNullOrEmpty(_selectQuery))
                {
                    var sb = new StringBuilder();
                    sb.Append($"SELECT t.{nameof(Team.id)} as {nameof(Id)}, t.{nameof(Team.name)} as {nameof(Name)}, t.{nameof(Team.logo)} as {nameof(Logo)}, t.{nameof(Team.country_name)} as {nameof(CountryName)} ");
                    //sb.Append($"c.{nameof(Country.logo)} as {nameof(CountryLogo)} ");
                    sb.Append("FROM team as t ");
                    //sb.Append($"LEFT JOIN country as c on t.{nameof(Team.country_name)} = c.{nameof(Country.name)} ");

                    _selectQuery = sb.ToString();
                }

                return _selectQuery;
            }
        }

        #endregion Select Query
    }
}