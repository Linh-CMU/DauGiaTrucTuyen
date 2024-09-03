using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    public class AccountDetail
    {
        [Key, ForeignKey("Account")]
        public string AccountID { get; set; }

        public virtual Account Account { get; set; }  

        public string? Avatar { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; }

        [StringLength(10)]
        public string? Phone { get; set; }
        public string? FrontCCCD { get; set; }
        public string? BacksideCCCD { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? Ward { get; set; }  

        [StringLength(100)]
        public string? District { get; set; } 

        [StringLength(250)]
        public string? Address { get; set; }
    }
}