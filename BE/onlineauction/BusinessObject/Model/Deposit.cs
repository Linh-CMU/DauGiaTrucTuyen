using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("Deposit")]
    public class Deposit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string DID { get; set; }
        [ForeignKey("RegistAuction")]
        public int RAID { get; set; }
        public string PaymentType { get; set; }
        public string PaymentDate { get; set; }
        public string status { get; set; }
        public virtual RegistAuction RegistAuctions { get; set; }
    }
}
