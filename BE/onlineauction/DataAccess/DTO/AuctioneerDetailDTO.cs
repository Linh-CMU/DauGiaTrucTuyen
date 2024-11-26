using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AuctionDetailDTO
    {
        public int ListAuctionID { get; set; }
        public string Category { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public string Image { get; set; }
        public string ImageEvidence { get; set; }
        public string NameAuction { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal? StepPrice { get; set; }
        public decimal PriceDeposit { get; set; }
        public string? StartDay { get; set; }
        public string? StartTime { get; set; }
        public string? EndDay { get; set; }
        public string? EndTime { get; set; }
        public string? TimePerLap { get; set; }
        public string StatusAuction { get; set; }
        public int countBidder { get; set; }
    }
}
