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
    public interface IAuctioneerRepository
    {
        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<AutionDTO> AuctionDetail(int id);
        /// <summary>
        /// Lists the auctioneer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        Task<List<ListAuctioneerDTO>> ListAuctioneer(int status, string uid);
        /// <summary>
        /// Updates the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="auctionDTO">The auction dto.</param>
        /// <returns></returns>
        Task<ResponseDTO> UpdateAuction(string id, UpdateAuctionDTO auctionDTO);
        /// <summary>
        /// Deletes the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> DeleteAuction(int id);
        /// <summary>
        /// Auctioneers the fl category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="status">The status.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        Task<List<ListAuctioneerDTO>> AuctioneerFlCategory(int category, int status, string uid);
        /// <summary>
        /// Searchs the auctioneer.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        Task<List<ListAuctioneerDTO>> SearchAuctioneer(string content, string uid, int categoryId);
        /// <summary>
        /// Listofregisteredbidderses the specified userid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusauction">The statusauction.</param>
        /// <returns></returns>
        Task<List<ListAuctioneerDTO>> Listofregisteredbidders(string userid, int status, bool? statusauction);
        /// <summary>
        /// Totals the pay.
        /// </summary>
        /// <param name="acutionId">The acution identifier.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        Task<InforPayMentDTO> TotalPay(int acutionId, string uid);
        /// <summary>
        /// Checks the pay ment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> CheckPayMent(Payment payment, int id);
        /// <summary>
        /// Sends the mail after paymet.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        SetTimeForBatchDTO sendMailAfterPaymet(int id, string uid);
        /// <summary>
        /// Lists the auctioneer by user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> ListAuctioneerByUser(string id, int status);
        Task<ResponseDTO> ListAuctioneerRegisterByUser(string id, int status);
        Task<SetTimeForBatch> GetInforSendMail(int id);
    }
}
