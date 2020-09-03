using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Mysql.PoseGlobalDB.Tables
{
    [Table("in_app_billing")]
    public class InAppBilling
    {
        [Key]
        [Column(TypeName = "BIGINT")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long trans_no { get; set; }

        [Required]
        [Column(TypeName = "BIGINT")]
        [Index("IDX_USER_NO", Order = 1, IsUnique = false)]
        public long user_no { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string store_type { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [Index("IDX_PRODUCT_ID", Order = 1, IsUnique = false)]
        [StringLength(48)]
        public string product_id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [Index("IDX_ORDER_ID", Order = 1, IsUnique = false)]
        [StringLength(64)]
        public string order_id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(1024)]
        public string purchase_token { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string purchase_state { get; set; }

        [Required]
        public DateTime upt_date { get; set; }

        [Required]
        public DateTime ipt_date { get; set; }
    }
}