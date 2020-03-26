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
    [Table("team")]
    public class Team
    {
        [Key]
        [Column(TypeName = "MEDIUMINT")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(64)]
        [Index("IDX_NAME_COUNTRY", Order = 1, IsUnique = false)]
        public string name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(32)]
        [Index("IDX_NAME_COUNTRY", Order = 2, IsUnique = false)]
        [Index("IDX_COUNTRY_NAME", IsUnique = false)]
        public string country_name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(100)]
        [SqlDefaultValue(DefaultValue = "")]
        public string logo { get; set; }

        [Required]
        public DateTime upt_time { get; set; }
    }
}