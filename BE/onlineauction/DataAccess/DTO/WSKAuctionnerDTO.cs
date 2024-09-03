using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class WSKAuctionnerDTO
    {
        public int ID { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }
        public decimal PriceStart { get; set; }
    }
}
