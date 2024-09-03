using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    
    public class UserService
    {
        private readonly IUserReponsitory _userReponsitory;
        private readonly IAuctioneerRepository _auctioneerRepository;
        public UserService(IUserReponsitory userReponsitory, IAuctioneerRepository auctioneerRepository)
        {
            _userReponsitory= userReponsitory;
            _auctioneerRepository = auctioneerRepository;
        }
        public async Task<ResponseDTO> RegiterAuctioneer(string userID, RegisterAuctioneerDTO register)
        {
            var result = await _userReponsitory.RegiterAuctioneer(userID, register);
            return result;
        }
        public async Task<ResponseDTO> UpdateAuctioneer(string id, UDAuctionDTO auctionDTO)
        {
            var result = await _auctioneerRepository.UpdateAuction(id, auctionDTO);
            return result;
        }
        public async Task<ResponseDTO> DeleteAuctioneer(int id)
        {
            var result = await _auctioneerRepository.DeleteAuction(id);
            return result;
        }
    }
}
