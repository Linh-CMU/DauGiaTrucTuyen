using BusinessObject.Model;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IUserReponsitory
    {
        Task<ResponseDTO> RegiterAuctioneer(string userID, RegisterAuctioneerDTO register);
    }
}
