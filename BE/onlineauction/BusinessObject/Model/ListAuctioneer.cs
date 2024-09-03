using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("ListAuctioneer")]
    public class ListAuctioneer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListAuctioneerID { get; set; }
        [ForeignKey("Account")]
        public string? Creator { get; set; }
        [ForeignKey("Account")]
        public string? Manager { get; set; }
        public string Image { get; set; }
        public string NameAuctioneer { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public bool? StatusAuction { get; set; }
        public virtual Account CreatorAccount { get; set; }
        public virtual Account ManagerAccount { get; set; }
        public virtual AuctioneerDetail AuctioneerDetails { get; set; }
        public virtual ICollection<RegistAuctioneer> RegistAuctioneers { get; set; } = new List<RegistAuctioneer>();
    }
}
