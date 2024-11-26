using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AcceptAutioneerDTO
    {
        public int AutioneerID { get; set; }
        public bool Status { get; set; }
        public decimal? PriceStep { get; set; }
        public string? TimeRoom { get; set; }
    }
}
