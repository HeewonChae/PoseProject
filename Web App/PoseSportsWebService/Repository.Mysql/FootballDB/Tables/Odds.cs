using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repository.Mysql.Contexts.FootballDB;

namespace Repository.Mysql.FootballDB.Tables
{
    [Table("odds")]
    public class Odds
    {
        [Key]
        [Column(Order = 1, TypeName = "INT")]
        public int fixture_id { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "VARCHAR")]
        [StringLength(32)]
        public string bookmaker_type { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "VARCHAR")]
        [StringLength(16)]
        public string label_type { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(8)]
        [SqlDefaultValue(DefaultValue = "")]
        public string subtitle_1 { get; set; }

        [Required]
        [Column(TypeName = "FLOAT")]
        [SqlDefaultValue(DefaultValue = "0")]
        public float odds_1 { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(8)]
        [SqlDefaultValue(DefaultValue = "")]
        public string subtitle_2 { get; set; }

        [Required]
        [Column(TypeName = "FLOAT")]
        [SqlDefaultValue(DefaultValue = "0")]
        public float odds_2 { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(8)]
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