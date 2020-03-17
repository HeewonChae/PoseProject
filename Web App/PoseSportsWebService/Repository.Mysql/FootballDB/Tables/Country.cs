using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Repository.Mysql.Contexts.FootballDB;

namespace Repository.Mysql.FootballDB.Tables
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
        public string logo { get; set; }

        [Required]
        public DateTime upt_time { get; set; }
    }
}