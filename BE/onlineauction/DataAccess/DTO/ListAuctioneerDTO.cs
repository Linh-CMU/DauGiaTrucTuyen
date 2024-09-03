﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ListAuctioneerDTO
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string StartDay { get; set; }
        public string StartTime { get; set; }
        public string EndDay { get; set; }  
        public string EndTime { get; set; }  
        public decimal PriceStart { get; set; }
    }
}
