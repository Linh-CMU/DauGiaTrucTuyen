using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ViewBidHistoryDTO
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public string DateAndTime { get; set; }
    }
}
