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
	[Table("country")]
	public class Country
	{
		[Key]
		[Column(Order = 1, TypeName = "CHAR")]
		[StringLength(4)]
		public string code { get; set; }

		[Key]
		[Column(Order = 2, TypeName = "VARCHAR")]
		[StringLength(32)]
		[Index("IDX_COUNTRY_NAME", IsUnique = false)]
		public string name { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(100)]
		[SqlDefaultValue(DefaultValue = "")]
		public string flag { get; set; }

		[Required]
		public DateTime upt_time { get; set; }
	}
}
