using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class VerifyOtpViewModel
    {
        public string Email { get; set; }  // Could also be the email, depending on your setup
        public string Otp { get; set; }
    }
}
