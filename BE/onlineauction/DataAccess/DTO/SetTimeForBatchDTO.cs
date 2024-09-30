using BusinessObject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class SetTimeForBatchDTO
    {
        public string EmailAdmin { get; set; }
        public string AuctioneerEmail { get; set; }
        public string BidderEmail { get; set; }
        public DateTime endTime { get; set; }
        public decimal Price { get; set; }
        public RegistAuction RegistAuctioneer { get; set; }
        public bool status { get; set; }
        public string AccountId { get; set; }
        public string Title { get; set; }
        public string AccountAdminId { get; set; }
        public string AccountAuctionId { get; set; }
    }
}
