using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
    public class AddressEntities
    {
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Street { get; set; }
    }

    public class VerificationData
    {
        public string Id { get; set; }
        public string Id_Prob { get; set; }
        public string Name { get; set; }
        public string Name_Prob { get; set; }
        public string Dob { get; set; }
        public string Dob_Prob { get; set; }
        public string Sex { get; set; }
        public string Sex_Prob { get; set; }
        public string Nationality { get; set; }
        public string Nationality_Prob { get; set; }
        public string Home { get; set; }
        public string Home_Prob { get; set; }
        public string Address { get; set; }
        public string Address_Prob { get; set; }
        public string Doe { get; set; }
        public string Doe_Prob { get; set; }
        public string Overall_Score { get; set; }
        public string Number_Of_Name_Lines { get; set; }
        public AddressEntities Address_Entities { get; set; }
        public string Type_New { get; set; }
        public string Type { get; set; }
    }

    public class VerificationResponse
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public List<VerificationData> Data { get; set; }
    }

}
