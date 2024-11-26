using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Model
{
    public class UserOtp
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Otp { get; set; }
        public DateTime ExpirationTime { get; set; }
        public int Attempts { get; set; }
        public virtual Account User { get; set; }
    }
}
