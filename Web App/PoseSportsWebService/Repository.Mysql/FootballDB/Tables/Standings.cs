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
    [Table("standings")]
    public class Standings
    {
        [Key]
        [Column(Order = 1, TypeName = "MEDIUMINT")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short league_id { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "MEDIUMINT")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short team_id { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "VARCHAR")]
        [StringLength(64)]
        [SqlDefaultValue(DefaultValue = "")]
        public string group { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short rank { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(8)]
        [SqlDefaultValue(DefaultValue = "")]
        public string forme { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short points { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(128)]
        [SqlDefaultValue(DefaultValue = "")]
        public string description { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short played { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short win { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short draw { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short lose { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short goals_for { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short goals_against { get; set; }

        [Required]
        public DateTime upt_time { get; set; }
    }
}