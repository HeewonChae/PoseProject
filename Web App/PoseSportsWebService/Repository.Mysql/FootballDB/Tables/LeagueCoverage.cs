using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.FootballDB.Tables
{
    [Table("league_coverage")]
    public class LeagueCoverage
    {
        [Key]
        [Column(TypeName = "MEDIUMINT")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short league_id { get; set; }

        [Required]
        public bool standings { get; set; }

        [Required]
        public bool odds { get; set; }

        [Required]
        public bool fixture_statistics { get; set; }

        [Required]
        public bool players { get; set; }

        [Required]
        public bool lineups { get; set; }

        [Required]
        public bool predictions { get; set; }

        [Required]
        public DateTime upt_time { get; set; }
    }
}