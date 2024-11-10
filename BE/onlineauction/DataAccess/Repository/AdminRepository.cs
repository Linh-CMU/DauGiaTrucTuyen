using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DataAccess.IRepository.IAdminRepository" />
    public class AdminRepository : IAdminRepository
    {
        /// <summary>
        /// The account manager
        /// </summary>
        private readonly UserManager<Account> _accountManager;
        /// <summary>
        /// The upload
        /// </summary>
        private readonly IUploadRepository _upload;
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRepository" /> class.
        /// </summary>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="upload">The upload.</param>
        public AdminRepository(UserManager<Account> accountManager, IUploadRepository upload)
        {
            _accountManager = accountManager;
            _upload = upload;
        }
        /// <summary>
        /// Accepts the auctioneer for admin.
        /// </summary>
        /// <param name="autioneer">The autioneer.</param>
        /// <param name="idAuction">The identifier auction.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idAuction)
        {
            try
            {
                await AuctionDAO.Instance.AcceptAuctioneerForAdmin(autioneer, idAuction);
                return new ResponseDTO { IsSucceed = true, Message = "successfully" };

            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed" };
            }
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AddCategory(string name)
        {
            try
            {
                var category = new Category
                {
                    NameCategory = name
                };
                var result = await CategoryDAO.Instance.AddCategory(category);
                if (result)
                {
                    return new ResponseDTO { IsSucceed = true, Message = "Add Category successfully" };
                }
                return new ResponseDTO { IsSucceed = false, Message = "Add Category failed" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Add Category failed" };
            }
        }

        /// <summary>
        /// Deletes the category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> DeleteCategory(int id)
        {
            try
            {
                var result = await CategoryDAO.Instance.DeleteCategory(id);
                if (result)
                {
                    return new ResponseDTO { IsSucceed = true, Message = "Delete Category successfully" };
                }
                return new ResponseDTO { IsSucceed = false, Message = "Delete Category failed" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Delete Category failed" };
            }
        }

        /// <summary>
        /// Lists the auction.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<List<AuctionDetailDTO>> ListAuction(string accountID, int status)
        {
            var result = await AuctionDAO.Instance.ListYourAuctioneerAdmin(accountID, status);
            return result;
        }
        /// <summary>
        /// Formats the time span.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns></returns>
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Days * 24 + timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
        /// <summary>
        /// Lists the category.
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseDTO> ListCategory()
        {
            try
            {
                var result = await CategoryDAO.Instance.CategoryAsync();
                return new ResponseDTO { Result = result, IsSucceed = true, Message = "List category successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "List category failed" };
            }
        }

        /// <summary>
        /// Updates the category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="Namecategory">The namecategory.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateCategory(int id, string Namecategory)
        {
            try
            {
                var category = new Category()
                {
                    CategoryID = id,
                    NameCategory = Namecategory
                };
                var result = await CategoryDAO.Instance.UpdateCategory(category);
                if (result)
                {
                    return new ResponseDTO { IsSucceed = true, Message = "Update category successfully" };
                }
                else
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Update category failed" };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Update category failed" };
            }
        }
        /// <summary>
        /// Profiles the user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ProfileUser(string username)
        {
            ProfileDTO profileDTO = null;
            Account account = null;
            AccountDetail accountDetail = null;
            account = await _accountManager.FindByIdAsync(username);
            var roles = await _accountManager.GetRolesAsync(account);
            var role = roles.FirstOrDefault(); // Get the first role
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account not found" };
            }

            accountDetail = await AccountDAO.Instance.ProfileDAO(account.Id);
            if (accountDetail == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Account details not found" };
            }
            profileDTO = new ProfileDTO
            {
                AccountId = account.Id,
                UserName = account.UserName,
                Avatar = accountDetail.Avatar,
                FrontCCCD = accountDetail.FrontCCCD,
                BacksideCCCD = accountDetail.BacksideCCCD,
                Email = account.Email,
                FullName = accountDetail.FullName,
                Phone = accountDetail.Phone,
                City = accountDetail.City,
                Ward = accountDetail.Ward,
                District = accountDetail.District,
                Address = accountDetail.Address,
                Status = account.Status,
                Role = role
            };
            return new ResponseDTO { Result = profileDTO, IsSucceed = true, Message = "Successfully" };
        }

        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AuctionDetail(int id)
        {
            try
            {
                var result = await AuctionDAO.Instance.AuctionDetail(id);
                return new ResponseDTO { Result = result, IsSucceed = true, Message = "Show auction detail successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Show auction detail failed" };
            }
        }

        /// <summary>
        /// Searchs the auctioneer admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<List<AuctionDetailDTO>> SearchAuctioneerAdmin(string id, string content)
        {
            var result = await AuctionDAO.Instance.SearchAuctioneerAdmin(id, content);
            return result;
        }

        /// <summary>
        /// Lists your auctioneer category admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public async Task<List<AuctionDetailDTO>> ListYourAuctioneerCategoryAdmin(string id, int status, int category)
        {
            var result = await AuctionDAO.Instance.ListYourAuctioneerCategoryAdmin(id, status, category);
            return result;
        }

        /// <summary>
        /// Auctions the detail batch job.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<DAuctioneerDTO> AuctionDetailBatchJob(int id)
        {
            var result = await AuctionDAO.Instance.AuctionDetail(id);
            return result;
        }

        public async Task<ResponseDTO> ReUpAuction(int id)
        {
            var result = await AuctionDAO.Instance.ReUpAuction(id);
            if (result)
            {
                return new ResponseDTO { IsSucceed = true, Message = "Successfully" };
            }
            else
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed" };

            }
        }

        public async Task<ResponseDTO> listBidderInAuction(int id)
        {
            var result = await AuctionDAO.Instance.listBidderInAuction(id);
            if (result != null)
            {
                return new ResponseDTO {Result = result, IsSucceed = true, Message = "Successfully" };
            }
            else
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed" };

            }
        }

        public async Task<AuctionRoomAdminDTO> AuctionRoomAdmin(int id)
        {
            var result = await AuctionDAO.Instance.AuctionRoomAdmin(id);
            return result;
        }
    }
}
