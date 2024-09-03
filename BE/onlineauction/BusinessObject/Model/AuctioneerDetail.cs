using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("AuctioneerDetail")]
    public class AuctioneerDetail
    {
        [Key, ForeignKey("ListAuctioneer")]
        public int ListAuctioneerID { get; set; }
        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public string StartDay { get; set; }
        public string StartTime { get; set; }
        public string EndDay { get; set; }
        public string EndTime { get; set; }
        public int NumberofAuctionRounds { get; set; }
        public string TimePerLap { get; set; }
        public decimal? PriceStep { get; set; }
        public string PaymentMethod { get; set; }
        public virtual ListAuctioneer ListAuctioneers { get; set; }
        public virtual Category Categorys { get; set; }
        public virtual ICollection<FileAttachments> FileAttachments { get; set; } = new List<FileAttachments>();
    }
}
