using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ApiReponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Token { get; set; }
    }
}
