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
    public interface IAdminRepository
    {
        /// <summary>
        /// Lists the auction.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        Task<List<AuctionDetailDTO>> ListAuction(string accountID, int status);
        /// <summary>
        /// Searchs the auctioneer admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        Task<List<AuctionDetailDTO>> SearchAuctioneerAdmin(string id, string content);
        /// <summary>
        /// Lists your auctioneer category admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        Task<List<AuctionDetailDTO>> ListYourAuctioneerCategoryAdmin(string id, int status, int category);
        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task<ResponseDTO> AddCategory(string name);
        /// <summary>
        /// Lists the category.
        /// </summary>
        /// <returns></returns>
        Task<ResponseDTO> ListCategory();
        /// <summary>
        /// Accepts the auctioneer for admin.
        /// </summary>
        /// <param name="autioneer">The autioneer.</param>
        /// <param name="idAuction">The identifier auction.</param>
        /// <returns></returns>
        Task<ResponseDTO> AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idAuction);
        /// <summary>
        /// Updates the category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="Namecategory">The namecategory.</param>
        /// <returns></returns>
        Task<ResponseDTO> UpdateCategory(int id, string Namecategory);
        /// <summary>
        /// Deletes the category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> DeleteCategory(int id);
        /// <summary>
        /// Profiles the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        Task<ResponseDTO> ProfileUser(string username);
        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> AuctionDetail(int id);

        /// <summary>
        /// Auctions the detail batch job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<DAuctioneerDTO> AuctionDetailBatchJob(int id);
        /// <summary>
        /// Res up auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> ReUpAuction(int id);
        /// <summary>
        /// Lists the bidder in auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ResponseDTO> listBidderInAuction(int id);
        /// <summary>
        /// Auctions the room admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<AuctionRoomAdminDTO> AuctionRoomAdmin(int id);
        /// <summary>
        /// Productstatisticses this instance.
        /// </summary>
        /// <returns></returns>
        Task<List<(string Day, int Count)>> Productstatistics();
        Task<List<(string Month, decimal Count)>> MonthlyIncomeStatistics();

    }
}
