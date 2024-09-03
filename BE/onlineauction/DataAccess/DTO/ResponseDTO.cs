using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class ResponseDTO
    {
        public object? Result { get; set; }
        public bool IsSucceed { get; set; }
        public string Message { get; set; } = "";
    }
}
