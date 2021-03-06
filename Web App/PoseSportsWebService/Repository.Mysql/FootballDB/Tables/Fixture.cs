﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Repository.Mysql.Contexts.FootballDB;

namespace Repository.Mysql.FootballDB.Tables
{
    [Table("fixture")]
    public class Fixture
    {
        [Key]
        [Column(TypeName = "INT")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        [Index("IDX_LEAGUE_DATE", Order = 1, IsUnique = false)]
        [Index("IDX_LEAGUE_HOME", Order = 1, IsUnique = false)]
        [Index("IDX_LEAGUE_AWAY", Order = 1, IsUnique = false)]
        public short league_id { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        [Index("IDX_HOME_DATE", Order = 1, IsUnique = false)]
        [Index("IDX_LEAGUE_HOME", Order = 2, IsUnique = false)]
        [Index("IDX_HOME_AWAY_DATE", Order = 1, IsUnique = false)]
        public short home_team_id { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        [Index("IDX_AWAY_DATE", Order = 1, IsUnique = false)]
        [Index("IDX_LEAGUE_AWAY", Order = 2, IsUnique = false)]
        [Index("IDX_HOME_AWAY_DATE", Order = 2, IsUnique = false)]
        public short away_team_id { get; set; }

        [Required]
        [Index("IDX_LEAGUE_DATE", Order = 2, IsUnique = false)]
        [Index("IDX_HOME_DATE", Order = 2, IsUnique = false)]
        [Index("IDX_AWAY_DATE", Order = 2, IsUnique = false)]
        [Index("IDX_PREDICTED_DATE", Order = 2, IsUnique = false)]
        [Index("IDX_COMPLETED_DATE", Order = 2, IsUnique = false)]
        [Index("IDX_HOME_AWAY_DATE", Order = 3, IsUnique = false)]
        [Index("IDX_MATCH_TIME", Order = 1, IsUnique = false)]
        public DateTime match_time { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(64)]
        public string round { get; set; }

        [Required]
        [Column(TypeName = "CHAR")]
        [StringLength(8)]
        public string status { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short home_score { get; set; }

        [Required]
        [Column(TypeName = "MEDIUMINT")]
        public short away_score { get; set; }

        [Required]
        [Index("IDX_COMPLETED_DATE", Order = 1, IsUnique = false)]
        public bool is_completed { get; set; }

        [Required]
        [Index("IDX_PREDICTED_DATE", Order = 1, IsUnique = false)]
        [SqlDefaultValue(DefaultValue = "0")]
        public bool is_predicted { get; set; }

        [Required]
        [SqlDefaultValue(DefaultValue = "0")]
        public bool is_recommended { get; set; }

        [Required]
        [SqlDefaultValue(DefaultValue = "0")]
        public bool has_odds { get; set; }

        [Required]
        [SqlDefaultValue(DefaultValue = "0")]
        public short max_grade { get; set; }

        [Required]
        public DateTime upt_time { get; set; }
    }
}