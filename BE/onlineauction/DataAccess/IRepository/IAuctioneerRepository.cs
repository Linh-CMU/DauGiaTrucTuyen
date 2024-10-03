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
        /// <returns></returns>
        Task<List<WSKAuctionnerDTO>> ListAuctioneer(int status);
        /// <summary>
        /// Updates the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="auctionDTO">The auction dto.</param>
        /// <returns></returns>
        Task<ResponseDTO> UpdateAuction(string id, UDAuctionDTO auctionDTO);
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
        /// <returns></returns>
        Task<List<WSKAuctionnerDTO>> AuctioneerFlCategory(int category, int status);
        /// <summary>
        /// Searchs the auctioneer.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<List<WSKAuctionnerDTO>> SearchAuctioneer(string content);
        /// <summary>
        /// Listofregisteredbidderses the specified userid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusauction">The statusauction.</param>
        /// <returns></returns>
        Task<List<WSKAuctionnerDTO>> Listofregisteredbidders(string userid, int status, bool? statusauction);
        /// <summary>
        /// Totals the pay.
        /// </summary>
        /// <param name="acutionId">The acution identifier.</param>
        /// <returns></returns>
        Task<InforPayMentDTO> TotalPay(int acutionId, string uid);
        /// <summary>
        /// Checks the pay ment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> CheckPayMent(Payment payment, int id);
        SetTimeForBatchDTO sendMailAfterPaymet(int id, string uid);

    }
}
