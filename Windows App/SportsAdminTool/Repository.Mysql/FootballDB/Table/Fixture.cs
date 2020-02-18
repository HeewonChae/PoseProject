using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repository.Mysql.Contexts.FootballDB;

namespace Repository.Mysql.FootballDB.Table
{
	[Table("fixture")]
	public class Fixture
	{
		[Key]
		[Column(TypeName = "INT")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int id { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		[Index("IDX_LEAGUE_DATE", Order = 1, IsUnique = false)]
		public short league_id { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		[Index("IDX_HOME_DATE", Order = 1, IsUnique = false)]
		public short home_team_id { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		[Index("IDX_AWAY_DATE", Order = 1, IsUnique = false)]
		public short away_team_id { get; set; }

		[Required]
		[Index("IDX_LEAGUE_DATE", Order = 2, IsUnique = false)]
		[Index("IDX_HOME_DATE", Order = 2, IsUnique = false)]
		[Index("IDX_AWAY_DATE", Order = 2, IsUnique = false)]
		public DateTime event_date { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(64)]
		[SqlDefaultValue(DefaultValue = "")]
		public string round { get; set; }

		[Required]
		[Column(TypeName = "CHAR")]
		[StringLength(8)]
		public string status { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		[SqlDefaultValue(DefaultValue = "0")]
		public short home_score { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		[SqlDefaultValue(DefaultValue = "0")]
		public short away_score { get; set; }

		[Required]
		[Index("IDX_PREDICTED", IsUnique = false)]
		public bool is_predicted { get; set; }

		[Required]
		[Index("IDX_COMPLETED", IsUnique = false)]
		public bool is_completed { get; set; }

		[Required]
		public DateTime upt_time { get; set; }
	}
}
