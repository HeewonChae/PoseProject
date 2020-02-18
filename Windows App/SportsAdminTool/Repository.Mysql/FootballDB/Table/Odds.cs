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
	[Table("odds")]
	public class Odds
	{
		[Key]
		[Column(TypeName = "BIGINT")]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long seq { get; set; }

		[Required]
		[Column(TypeName = "INT")]
		[Index("IDX_FIXTURE_LABEL", Order = 1, IsUnique = false)]
		public int fixture_id { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short bookmaker_id { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		[Index("IDX_FIXTURE_LABEL", Order = 2, IsUnique = false)]
		public short label_id { get; set; }

		[Required]
		[Column(TypeName = "VARCHAR")]
		[StringLength(16)]
		[SqlDefaultValue(DefaultValue = "")]
		public string subtitle_1 { get; set; }

		[Required]
		[Column(TypeName = "FLOAT")]
		[SqlDefaultValue(DefaultValue = "0")]
		public float odds_1 { get; set; }

		[Required]
		[Column(TypeName = "VARCHAR")]
		[StringLength(16)]
		[SqlDefaultValue(DefaultValue = "")]
		public string subtitle_2 { get; set; }

		[Required]
		[Column(TypeName = "FLOAT")]
		[SqlDefaultValue(DefaultValue = "0")]
		public float odds_2 { get; set; }

		[Required]
		[Column(TypeName = "VARCHAR")]
		[StringLength(16)]
		[SqlDefaultValue(DefaultValue = "")]
		public string subtitle_3 { get; set; }

		[Required]
		[Column(TypeName = "FLOAT")]
		[SqlDefaultValue(DefaultValue = "0")]
		public float odds_3 { get; set; }

		[Required]
		public DateTime upt_time { get; set; }
	}
}
