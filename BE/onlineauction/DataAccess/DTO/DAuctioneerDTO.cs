using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class DAuctioneerDTO
    {
        public int ID { get; set; }
        public object User { get; set; }
        public string Manager { get; set; }
        public string Image { get; set; }
        public string NameAuctioneer { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public string categoryName { get; set; }
        public string StartDay { get; set; }
        public string StartTime { get; set; }
        public string EndDay { get; set; }
        public string EndTime { get; set; }
        public int NumberofAuctionRounds { get; set; }
        public string TimePerLap { get; set; }
        public decimal? PriceStep { get; set; }
        public string PaymentMethod { get; set; }
        public string FileAuctioneer { get; set; }
        public string SignatureImg { get; set; }
        public object TImange { get; set; }
        public object WinBidder { get; set; }
        public string StatusAuction { get; set; }
    }
}
