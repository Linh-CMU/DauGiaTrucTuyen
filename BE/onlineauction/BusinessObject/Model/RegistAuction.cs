using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("RegistAuction")]
    public class RegistAuction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RAID { get; set; }

        [ForeignKey("Account")]
        public string AccountID { get; set; }

        [ForeignKey("ListAuction")]
        public int ListAuctionID { get; set; }
        public string? PaymentTerm { get; set; }
        public bool? AuctionStatus { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();
        public virtual ICollection<Bet> Bets { get; set; } = new List<Bet>();

        public virtual ListAuction ListAuctions { get; set; }

        public virtual Feedback Feedbacks { get; set; } 

        public virtual Account Accounts { get; set; }
    }
}
