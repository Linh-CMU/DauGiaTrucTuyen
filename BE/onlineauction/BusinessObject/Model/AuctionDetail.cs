using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("AuctionDetail")]
    public class AuctionDetail
    {
        [Key, ForeignKey("ListAuction")]
        public int ListAuctionID { get; set; }
        [ForeignKey("Category")]
        public int? CategoryID { get; set; }
        public string? StartDay { get; set; }
        public string? StartTime { get; set; }
        public string? EndDay { get; set; }
        public string? EndTime { get; set; }
        public int? NumberofAuctionRounds { get; set; }
        public string? TimePerLap { get; set; }
        public decimal? PriceStep { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public virtual ListAuction ListAuctions { get; set; }
        public virtual Category? Categorys { get; set; }
        public virtual ICollection<TImage> TImages { get; set; } = new List<TImage>();
    }
}
