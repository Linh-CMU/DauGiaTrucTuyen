using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AddInformationDTO
    {
        public IFormFile? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public IFormFile? FrontCCCD { get; set; }
        public IFormFile? BacksideCCCD { get; set; }
        public string? City { get; set; }
        public string? Ward { get; set; }
        public string? District { get; set; }
        public string? Address { get; set; }
    }
}
