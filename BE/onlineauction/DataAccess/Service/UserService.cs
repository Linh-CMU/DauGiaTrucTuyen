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

    /// <summary>
    /// 
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// The user reponsitory
        /// </summary>
        private readonly IUserReponsitory _userReponsitory;
        /// <summary>
        /// The auctioneer repository
        /// </summary>
        private readonly IAuctioneerRepository _auctioneerRepository;
        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userReponsitory">The user reponsitory.</param>
        /// <param name="auctioneerRepository">The auctioneer repository.</param>
        public UserService(IUserReponsitory userReponsitory, IAuctioneerRepository auctioneerRepository)
        {
            _userReponsitory= userReponsitory;
            _auctioneerRepository = auctioneerRepository;
        }
        /// <summary>
        /// Regiters the auctioneer.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="register">The register.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> RegiterAuctioneer(string userID, RegisterAuctioneerDTO register)
        {
            var result = await _userReponsitory.RegiterAuctioneer(userID, register);
            return result;
        }
        /// <summary>
        /// Updates the auctioneer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="auctionDTO">The auction dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateAuctioneer(string id, UDAuctionDTO auctionDTO)
        {
            var result = await _auctioneerRepository.UpdateAuction(id, auctionDTO);
            return result;
        }
        /// <summary>
        /// Deletes the auctioneer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> DeleteAuctioneer(int id)
        {
            var result = await _auctioneerRepository.DeleteAuction(id);
            return result;
        }
        /// <summary>
        /// Users the register auction.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="auctionId">The auction identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UserRegisterAuction(string userID, int auctionId)
        {
            var result = await _userReponsitory.UserRegisterAuction(userID, auctionId);
            return result;
        }
        /// <summary>
        /// Places the bid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="auctionId">The auction identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> PlaceBid(string userid, int auctionId)
        {
            var result = await _userReponsitory.PlaceBid(userid, auctionId);
            return result;
        }
        /// <summary>
        /// Lists your auctioneer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListYourAuctioneer(string id, int status)
        {
            var result = await _userReponsitory.ListYourAuctioneer(id, status);
            return result;
        }
        /// <summary>
        /// Lists your autioneer detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListYourAutioneerDetail(int id)
        {
            var result = await _userReponsitory.ListYourAutioneerDetail(id);
            return result;
        }
        /// <summary>
        /// Auctionrooms the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<AuctionRoomDTO> Auctionroom(int id)
        {
            var result = await _userReponsitory.Auctionroom(id);
            return result;
        }
        /// <summary>
        /// Withdraws the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> Withdraw(int id)
        {
            var result = await _userReponsitory.Withdraw(id);
            return result;
        }
        /// <summary>
        /// Views the bid history.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<List<ViewBidHistoryDTO>> ViewBidHistory(int id)
        {
            var result = await _userReponsitory.ViewBidHistory(id);
            return result;
        }
        public async Task<bool> PaymentForDeposit(Deposit deposit)
        {
            var result = await _userReponsitory.PaymentForDeposit(deposit);
            return result;
        }
        public async Task<int> getIdRegisterAuction(int id)
        {
            var result = await _userReponsitory.getIdRegisterAuction(id);
            return result;
        }
        public async Task<InforPayMentDTO> TotalPayDeposit(int acutionId, string uid)
        {
            var result = await _userReponsitory.TotalPayDeposit(acutionId, uid);
            return result;
        }
    }
}
