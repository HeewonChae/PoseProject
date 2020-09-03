using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Tables
{
    [Table("user_base")]
    public class UserBase
    {
        [Key]
        [Column(TypeName = "BIGINT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long user_no { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [Index("IDX_PLATFORM_ID", Order = 1, IsUnique = true)]
        [StringLength(128)]
        public string platform_id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string platform_type { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Index("IDX_PLATFORM_EMAIL", Order = 1, IsUnique = false)]
        [StringLength(64)]
        public string platform_email { get; set; }

        [Required]
        public DateTime last_login_date { get; set; }

        [Required]
        public DateTime ipt_date { get; set; }
    }
}