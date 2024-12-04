using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class FormAddAuctionDTO
    {
        public IFormFile? imageAuction { get; set; }
        public string nameAuction { get; set; }
        public string description { get; set; }
        public decimal startingPrice { get; set; }
        public int categoryID { get; set; }
        public IFormFile? imageVerification { get; set; }
    }
}
