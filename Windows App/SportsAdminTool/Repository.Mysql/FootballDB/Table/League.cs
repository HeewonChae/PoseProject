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
	[Table("league")]
	public class League
	{
		[Key]
		[Column(TypeName = "MEDIUMINT")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public short id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(64)]
		[Index("IDX_LEAGUE_NAME", IsUnique = false)]
		public string name { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(8)]
		public string type { get; set; }

		[Required]
		[Column(TypeName = "VARCHAR")]
		[StringLength(32)]
		[Index("IDX_COUNTRY_NAME", IsUnique = false)]
		public string country_name { get; set; }

		[Required]
		public bool is_current { get; set; }

		[Required]
		public DateTime season_start { get; set; }

		[Required]
		[Index("IDX_SEASON_END", IsUnique = false)]
		public DateTime season_end { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(64)]
		[SqlDefaultValue(DefaultValue = "")]
		public string logo { get; set; }

		[Required]
		public DateTime upt_time { get; set; }
	}
}
