using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("FileAttachments")]
    public class FileAttachments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileAID { get; set; }
        [ForeignKey("AuctioneerDetail")]
        public int ListAuctioneerID { get; set; }
        public string FileAuctioneer { get; set; }
        public string SignatureImg { get; set; }
        public virtual AuctioneerDetail AuctioneerDetails { get; set; }
        public virtual ICollection<TImage> TImages { get; set; } = new List<TImage>();
    }
}
