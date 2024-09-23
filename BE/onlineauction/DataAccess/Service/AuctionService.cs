using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionService
    {
        /// <summary>
        /// The auctioneer repository
        /// </summary>
        private readonly IAuctioneerRepository _auctioneerRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionService"/> class.
        /// </summary>
        /// <param name="auctioneerRepository">The auctioneer repository.</param>
        public AuctionService(IAuctioneerRepository auctioneerRepository) 
        {
            _auctioneerRepository = auctioneerRepository;
        }
        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<AutionDTO> AuctionDetail(int id)
        {
            var result = await _auctioneerRepository.AuctionDetail(id);
            return result;
        }
        /// <summary>
        /// Lists the auctioneer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<List<WSKAuctionnerDTO>> ListAuctioneer(int status)
        {
            var result = await _auctioneerRepository.ListAuctioneer(status);
            return result;
        }
        /// <summary>
        /// Searchs the auctioneer.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<List<WSKAuctionnerDTO>> SearchAuctioneer(string content)
        {
            var result = await _auctioneerRepository.SearchAuctioneer(content);
            return result;
        }
        /// <summary>
        /// Auctioneers the fl category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<List<WSKAuctionnerDTO>> AuctioneerFlCategory(int category, int status)
        {
            var result = await _auctioneerRepository.AuctioneerFlCategory(category, status);
            return result;
        }
        /// <summary>
        /// Listofregisteredbidderses the specified userid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusauction">The statusauction.</param>
        /// <returns></returns>
        public async Task<List<WSKAuctionnerDTO>> Listofregisteredbidders(string userid, int status, bool? statusauction)
        {
            var result = await _auctioneerRepository.Listofregisteredbidders(userid, status, statusauction);
            return result;
        }
        /// <summary>
        /// Totals the pay.
        /// </summary>
        /// <param name="acutionId">The acution identifier.</param>
        /// <returns></returns>
        public decimal TotalPay(int acutionId)
        {
            var result = _auctioneerRepository.TotalPay(acutionId);
            return result;
        }
        /// <summary>
        /// Checks the pay ment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> CheckPayMent(Payment payment, int id)
        {
            return await _auctioneerRepository.CheckPayMent(payment, id);
        }
    }
}
