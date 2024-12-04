using BusinessObject.Model;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserReponsitory
    {
        /// <summary>
        /// Regiters the auctioneer.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="register">The register.</param>
        /// <returns></returns>
        Task<ResponseDTO> RegiterAuctioneer(string userID, RegisterAuctioneerDTO register);
        /// <summary>
        /// Users the register auction.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="auctionId">The auction identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> UserRegisterAuction(string userID, int auctionId);
        /// <summary>
        /// Places the bid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> PlaceBid(string userid, RaiseDTO auction);
        /// <summary>
        /// Lists your auctioneer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        Task<ResponseDTO> ListYourAuctioneer(string id, int status);
        /// <summary>
        /// Lists your autioneer detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> ListYourAutioneerDetail(int id);
        /// <summary>
        /// Auctionrooms the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<AuctionRoomDTO> Auctionroom(int id, string userId);
        /// <summary>
        /// Withdraws the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> Withdraw(int id);
        /// <summary>
        /// Views the bid history.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<List<ViewBidHistoryDTO>> ViewBidHistory(int id);
        /// <summary>
        /// Payments for deposit.
        /// </summary>
        /// <param name="deposit">The deposit.</param>
        /// <returns></returns>
        Task<bool> PaymentForDeposit(Deposit deposit);
        /// <summary>
        /// Gets the identifier register auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<int> getIdRegisterAuction(int id);
        /// <summary>
        /// Totals the pay deposit.
        /// </summary>
        /// <param name="acutionId">The acution identifier.</param>
        /// <returns></returns>
        Task<InforPayMentDTO> TotalPayDeposit(int acutionId, string uid);
        Task<bool> Payment(Payment deposit);
        Task<ResponseDTO> UpdatePayment(string id, string status);
        Task<ResponseDTO> SearchListYourAuctioneer(string id, int status, string content);
    }
}
