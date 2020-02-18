using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Table
{
	[Table("fixture_statistic")]
	public class FixtureStatistic
	{
		[Key]
		[Column(Order = 1, TypeName = "INT")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int fixture_id { get; set; }

		[Key]
		[Column(Order = 2, TypeName = "MEDIUMINT")]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public short team_id { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short shots_total{ get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short shots_on_goal { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short shots_off_goal { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short shots_blocked { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short shots_inside_box { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short shots_outside_box { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short goalkeeper_saves { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short fouls { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short corner_kicks { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short offsides { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short yellow_cards { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short red_cards { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short ball_possessions { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short passes_total { get; set; }

		[Required]
		[Column(TypeName = "MEDIUMINT")]
		public short passes_accurate { get; set; }

		[Required]
		public DateTime upt_time { get; set; }
	}
}
