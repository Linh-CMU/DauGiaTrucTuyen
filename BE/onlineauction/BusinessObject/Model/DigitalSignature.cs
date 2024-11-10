using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    [Table("DigitalSignature")]
    public class DigitalSignature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FileAID { get; set; }
        [ForeignKey("AuctionDetail")]
        public int ListAuctionID { get; set; }
        public string Base64SignatureImage { get; set; }
        public string SignatureImg { get; set; }
        public string Signature { get; set; } // Chữ ký số
        public string PublicKey { get; set; } // Khóa công khai dùng để xác minh
        public string PrivateKey { get; set; } // Khóa riêng tư dùng để ký (có thể không lưu trữ vì lý do bảo mật)
        public DateTime CreatedAt { get; set; }
        public virtual AuctionDetail AuctionDetails { get; set; }
        public virtual ICollection<TImage> TImages { get; set; } = new List<TImage>();
    }
}
