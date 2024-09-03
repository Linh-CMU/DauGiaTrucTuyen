using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class UpdatePermissionDTO
    {
        [Required(ErrorMessage = "UserName is required")]
        public string Username { get; set; }
    }
}
