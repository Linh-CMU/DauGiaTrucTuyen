using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    public class AuctionService
    {
        private readonly IAuctioneerRepository _auctioneerRepository;
        public AuctionService(IAuctioneerRepository auctioneerRepository) 
        {
            _auctioneerRepository = auctioneerRepository;
        }
        public async Task<AutionDTO> AuctionDetail(int id)
        {
            var result = await _auctioneerRepository.AuctionDetail(id);
            return result;
        }
        public async Task<List<WSKAuctionnerDTO>> ListAuctioneer(int status)
        {
            var result = await _auctioneerRepository.ListAuctioneer(status);
            return result;
        }
        public async Task<List<WSKAuctionnerDTO>> SearchAuctioneer(string content)
        {
            var result = await _auctioneerRepository.SearchAuctioneer(content);
            return result;
        }
        public async Task<List<WSKAuctionnerDTO>> AuctioneerFlCategory(int category, int status)
        {
            var result = await _auctioneerRepository.AuctioneerFlCategory(category, status);
            return result;
        }
    }
}
