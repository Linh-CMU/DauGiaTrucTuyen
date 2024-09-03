using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoticationID { get; set; }
        [ForeignKey("Account")]
        public string AccountID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool StatusNotification { get; set; }
        public virtual Account Accounts { get; set; }
    }
}
