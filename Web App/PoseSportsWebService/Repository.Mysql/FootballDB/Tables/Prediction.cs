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
    [Table("prediction")]
    public class Prediction
    {
        [Key]
        [Column(Order = 1, TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int fixture_id { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "MEDIUMINT")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short main_label { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "MEDIUMINT")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short sub_label { get; set; }

        [SqlDefaultValue(DefaultValue = "0")]
        public int value1 { get; set; }

        [SqlDefaultValue(DefaultValue = "0")]
        public int value2 { get; set; }

        [SqlDefaultValue(DefaultValue = "0")]
        public int value3 { get; set; }

        [SqlDefaultValue(DefaultValue = "0")]
        public int value4 { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short grade { get; set; }

        [Required]
        public bool is_recommended { get; set; }

        [Required]
        public bool is_hit { get; set; }

        [Required]
        public DateTime upt_time { get; set; }
    }
}