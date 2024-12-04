using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("PlacingABid")]
    public class PlacingABid
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlacingABidID { get; set; }

        [ForeignKey("RegistAuction")]
        public int RAID { get; set; }
        public decimal PriceBit { get; set; }
        public DateTime BidTime { get; set; }
        public virtual RegistAuction RegistAuctioneer { get; set; }
    }
}
