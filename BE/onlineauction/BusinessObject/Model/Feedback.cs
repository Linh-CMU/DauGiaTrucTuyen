using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("Feedback")]
    public class Feedback
    {
        [Key]
        public int RAID { get; set; }

        [ForeignKey("RAID")]
        public virtual RegistAuction RegistAuctions { get; set; }

        [ForeignKey("Account")]
        public string AccountID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string SatisfactionLevel { get; set; }

        public virtual Account Accounts { get; set; }
    }
}
