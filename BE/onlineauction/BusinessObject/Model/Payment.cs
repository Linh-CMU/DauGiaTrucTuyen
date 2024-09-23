using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayID { get; set; }
        [ForeignKey("RegistAuction")]
        public int RAID { get; set; }
        public string PaymentType { get; set; }
        public string PaymentDate { get; set; }
        public virtual RegistAuction RegistAuctions { get; set; }
    }
}
