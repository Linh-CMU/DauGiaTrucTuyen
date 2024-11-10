using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("TImage")]
    public class TImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TImageId { get; set; }
        [ForeignKey("FileAttachments")]
        public int FileAID { get; set; }
        public string Imange { get; set; }
        public virtual DigitalSignature FileAttachments { get; set; }

    }
}
