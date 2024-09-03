using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    public interface IAuctioneerRepository
    {
        Task<AutionDTO> AuctionDetail(int id);
        Task<List<WSKAuctionnerDTO>> ListAuctioneer(int status);
        Task<ResponseDTO> UpdateAuction(string id, UDAuctionDTO auctionDTO);
        Task<ResponseDTO> DeleteAuction(int id);
        Task<List<WSKAuctionnerDTO>> AuctioneerFlCategory(int category, int status);
        Task<List<WSKAuctionnerDTO>> SearchAuctioneer(string content);
    }
}
