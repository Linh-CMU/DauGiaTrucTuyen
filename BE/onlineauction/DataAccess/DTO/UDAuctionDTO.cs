using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class UDAuctionDTO
    {
        public int AuctionID { get; set; }
        public IFormFile Image { get; set; }
        public string NameAuctioneer { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
    }
}
