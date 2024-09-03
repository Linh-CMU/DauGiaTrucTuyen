using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ResetPasswordDTO
    {
        public string UsernameOrEmail { get; set; } // Có thể là username hoặc email
        public string ResetToken { get; set; }      // Token reset mật khẩu
        public string NewPassword { get; set; }     // Mật khẩu mới
    }
}
