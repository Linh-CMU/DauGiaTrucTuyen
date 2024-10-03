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
        public async Task<ResponseDTO> AuctionDetail(int id)
        {
            try
            {
                var result = await _auctioneerRepository.AuctionDetail(id);
                var response = new ResponseDTO()
                {
                    Result = result,
                    IsSucceed = true,
                    Message = "Success"
                };
                return response;
            }
            catch
            {
                var response = new ResponseDTO()
                {
                    IsSucceed = false,
                    Message = "false"
                };
                return response;
            }
        }
        /// <summary>
        /// Lists the auctioneer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListAuctioneer(int status)
        {
            try
            {
                var result = await _auctioneerRepository.ListAuctioneer(status);
                var response = new ResponseDTO()
                {
                    Result = result,
                    IsSucceed = true,
                    Message = "Success"
                };
                return response;
            }
            catch
            {
                var response = new ResponseDTO()
                {
                    IsSucceed = false,
                    Message = "false"
                };
                return response;
            }
        }
        /// <summary>
        /// Searchs the auctioneer.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> SearchAuctioneer(string content)
        {
            try
            {
                var result = await _auctioneerRepository.SearchAuctioneer(content);
                var response = new ResponseDTO()
                {
                    Result = result,
                    IsSucceed = true,
                    Message = "Success"
                };
                return response;
            }
            catch
            {
                var response = new ResponseDTO()
                {
                    IsSucceed = false,
                    Message = "false"
                };
                return response;
            }
        }
        /// <summary>
        /// Auctioneers the fl category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> AuctioneerFlCategory(int category, int status)
        {
            try
            {
                var result = await _auctioneerRepository.AuctioneerFlCategory(category, status);
                var response = new ResponseDTO()
                {
                    Result = result,
                    IsSucceed = true,
                    Message = "Success"
                };
                return response;
            }
            catch
            {
                var response = new ResponseDTO()
                {
                    IsSucceed = false,
                    Message = "false"
                };
                return response;
            }
        }
        /// <summary>
        /// Listofregisteredbidderses the specified userid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusauction">The statusauction.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> Listofregisteredbidders(string userid, int status, bool? statusauction)
        {
            try
            {
                var result = await _auctioneerRepository.Listofregisteredbidders(userid, status, statusauction);
                var response = new ResponseDTO()
                {
                    Result = result,
                    IsSucceed = true,
                    Message = "Success"
                };
                return response;
            }
            catch
            {
                var response = new ResponseDTO()
                {
                    IsSucceed = false,
                    Message = "false"
                };
                return response;
            }
        }
        /// <summary>
        /// Totals the pay.
        /// </summary>
        /// <param name="acutionId">The acution identifier.</param>
        /// <returns></returns>
        public async Task<InforPayMentDTO> TotalPay(int acutionId, string uid)
        {
            var result = await _auctioneerRepository.TotalPay(acutionId, uid);
            return result;
        }
        /// <summary>
        /// Checks the pay ment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> CheckPayMent(Payment payment, int id, string uid)
        {
            var result = _auctioneerRepository.sendMailAfterPaymet(id, uid);
            var notificationForBidder = new Notification
            {
                AccountID = result.AccountId,
                Title = $"Thanh Toán thành công: {result.Title}",
                Description = "Cảm ơn bạn đã tin tưởng và sử dụng dịch vụ của chúng tôi"
            };
            await NotificationDAO.Instance.AddNotification(notificationForBidder);

            // Thông báo cho admin
            var adminNotification = new Notification
            {
                AccountID = result.AccountAdminId,
                Title = $"Thanh Toán thành công: {result.Title}",
                Description = $"Người thắng cuộc: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n và đã thanh toán thành công"
            };
            var auctioneerNotification = new Notification
            {
                AccountID = result.AccountAuctionId,
                Title = $"Thanh Toán thành công: {result.Title}",
                Description = $"Người thắng cuộc: {result.BidderEmail}\nGiá thắng cuộc: {result.Price}\n và đã thanh toán thành công"
            };
            await NotificationDAO.Instance.AddNotification(auctioneerNotification);
            await NotificationDAO.Instance.AddNotification(adminNotification);
            return await _auctioneerRepository.CheckPayMent(payment, id);
        }
    }
}
