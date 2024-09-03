using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Model
{
    [Table("Account")]
    public class Account : IdentityUser // Sử dụng IdentityUser<int> nếu bạn muốn AccountId là kiểu int
    {
        // IdentityUser đã có sẵn UserName, Email, và các thuộc tính khác
        // Không cần khai báo lại UserName và Email ở đây

        [Required]
        public int Warning { get; set; }

        public bool? Status { get; set; }

        public virtual AccountDetail AccountDetail { get; set; }  // Sửa thành AccountDetail

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        public virtual ICollection<RegistAuctioneer> RegistAuctioneers { get; set; } = new List<RegistAuctioneer>();

        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public virtual ICollection<ListAuctioneer> CreatedAuctioneers { get; set; } = new List<ListAuctioneer>();

        public virtual ICollection<ListAuctioneer> ManagedAuctioneers { get; set; } = new List<ListAuctioneer>();
    }
}