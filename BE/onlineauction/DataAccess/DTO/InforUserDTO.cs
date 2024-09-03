using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class InforUserDTO
    {
        public string AccountId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string? FrontCCCD { get; set; }
        public string? BacksideCCCD { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public int Warning { get; set; }
        public bool? Status { get; set; }
        public string Role { get; set; }
    }
}
