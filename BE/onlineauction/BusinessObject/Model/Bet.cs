using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("Bet")]
    public class Bet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BetID { get; set; }

        [ForeignKey("RegistAuction")]
        public int RAID { get; set; }
        public decimal PriceBit { get; set; }
        public string BidTime { get; set; }
        public virtual RegistAuction RegistAuctioneer { get; set; }
    }
}
