using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class SignRequest
    {
        public IFormFile SignatureImage { get; set; } // Hình ảnh chữ ký
        public string PrivateKey { get; set; }
    }

    public class VerifyRequest
    {
        public IFormFile SignatureImage { get; set; } // Hình ảnh chữ ký
        public string Signature { get; set; }
        public string PublicKey { get; set; }
    }
}
