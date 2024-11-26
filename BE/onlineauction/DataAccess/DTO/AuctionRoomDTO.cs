using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AuctionRoomDTO
    {
        public int ListAuctionID { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public string NameAuction { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal? stepMoney { get; set; }
        public string StartDay { get; set; }
        public string StartTime { get; set; }
        public string EndDay { get; set; }
        public string EndTime { get; set; }
        public string TimeRound { get; set; }
        public string bidStatus { get; set; }
        public bool statusBet { get; set; }
    }
}
