using BusinessObject.Model;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IAdminRepository
    {
        Task<List<AuctionnerAdminDTO>> ListAuction(string accountID, int status);
        Task<List<AuctionnerAdminDTO>> SearchAuctioneerAdmin(string id, string content);
        Task<List<AuctionnerAdminDTO>> ListYourAuctioneerCategoryAdmin(string id, int status, int category);
        Task<ResponseDTO> AddCategory(string name);
        Task<ResponseDTO> ListCategory();
        Task<ResponseDTO> AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idAuction);
        Task<ResponseDTO> UpdateCategory(int id, string Namecategory);
        Task<ResponseDTO> DeleteCategory(int id);
        Task<ResponseDTO> ProfileUser(string username);
        Task<ResponseDTO> AuctioneerDetail(int id);

    }
}
