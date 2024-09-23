using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("ListAuction")]
    public class ListAuction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListAuctionID { get; set; }
        [ForeignKey("Account")]
        public string? Creator { get; set; }
        [ForeignKey("Account")]
        public string? Manager { get; set; }
        public string Image { get; set; }
        public string NameAuction { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public bool? StatusAuction { get; set; }
        public virtual Account CreatorAccount { get; set; }
        public virtual Account ManagerAccount { get; set; }
        public virtual AuctionDetail AuctionDetails { get; set; }
        public virtual ICollection<RegistAuction> RegistAuctions { get; set; } = new List<RegistAuction>();
    }
}
