using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AddInforUserDTO
    {
        public IFormFile? avatar { get; set; }
        public string? fullName { get; set; }
        public string? phone { get; set; }
        public IFormFile? frontCCCD { get; set; }
        public IFormFile? backsideCCCD { get; set; }
        public IFormFile? signature { get; set; }
        public string? city { get; set; }
        public string? ward { get; set; }
        public string? district { get; set; }
        public string? address { get; set; }
        public string? birthdate { get; set; }
        public bool? gender { get; set; }
        public string? placeOfResidence { get; set; }
        public string? placeOfIssue { get; set; }
        public string? dateOfIssue { get; set; }

    }
}
