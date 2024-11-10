using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AuctionRoomAdminDTO
    {
        public int id { get; set; }
        public object user { get; set; }
        public string manager { get; set; }
        public string image { get; set; }
        public string nameAuction { get; set; }
        public decimal startingPrice { get; set; }
        public string categoryName { get; set; }
        public string startDay { get; set; }
        public string startTime { get; set; }
        public string endDay { get; set; }
        public string endTime { get; set; }
        public decimal? priceStep { get; set; }
        public object? winBidder { get; set; }
        public decimal moneyDeposit { get; set; }
        public decimal? bidMoney { get; set; }
        public int numberofAuctionRounds { get; set; }
        public string paymentMethod { get; set; }
        public string StatusAuction { get; set; }
    }
}
