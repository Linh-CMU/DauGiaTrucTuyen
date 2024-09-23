using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class RegisterAuctioneerDTO
    {
        public IFormFile Image { get; set; }
        public string NameAuction { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public int CategoryID { get; set; }
        public string StartDay { get; set; }
        public string StartTime { get; set; }
        public string EndDay { get; set; }
        public string EndTime { get; set; }
        public IFormFile file { get; set; }
        public IFormFile signatureImg { get; set; }
        public IFormFile image { get; set; }
    }
}
