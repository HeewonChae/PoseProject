using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Tables
{
    [Table("user_role")]
    public class UserRole
    {
        [Key]
        [Column(TypeName = "BIGINT")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long user_no { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string role_type { get; set; }

        [Required]
        [Column(TypeName = "BIGINT")]
        public long linked_trans_no { get; set; }

        [Required]
        public DateTime expire_time { get; set; }

        [Required]
        public DateTime upt_date { get; set; }
    }
}