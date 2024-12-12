using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using DataAccess.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DataAccess.IRepository.IAuctioneerRepository" />
    public class AuctioneerRepository : IAuctioneerRepository
    {
        /// <summary>
        /// The upload
        /// </summary>
        private readonly IUploadRepository _upload;
        /// <summary>
        /// The account manager
        /// </summary>
        private readonly UserManager<Account> _accountManager;
        /// <summary>
        /// Initializes a new instance of the <see cref="AuctioneerRepository" /> class.
        /// </summary>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="upload">The upload.</param>
        public AuctioneerRepository(UserManager<Account> accountManager, IUploadRepository upload)
        {
            _accountManager = accountManager;
            _upload = upload;
        }
        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<AutionDTO> AuctionDetail(int id)
        {
            var result = await AuctionDAO.Instance.AuctionDetail(id);
            if (result.StatusAuction != "Not approved yet")
            {
                bool hasStartDay = !string.IsNullOrEmpty(result.StartDay);
                bool hasStartTime = !string.IsNullOrEmpty(result.StartTime);
                bool hasEndDay = !string.IsNullOrEmpty(result.EndDay);
                bool hasEndTime = !string.IsNullOrEmpty(result.EndTime);

                var isOngoing = hasStartDay && hasStartTime &&
                                DateTime.ParseExact(result.StartDay, "dd/MM/yyyy", null) <= DateTime.Today &&
                                TimeSpan.Parse(result.StartTime) <= DateTime.Now.TimeOfDay;

                var auctionDate = isOngoing ? result.EndDay : result.StartDay;
                var auctionTime = isOngoing ? result.EndTime : result.StartTime;

                string formattedTimeRemaining = "";

                // Kiểm tra nếu cả ngày và giờ đều có giá trị
                if (!string.IsNullOrEmpty(auctionDate) && !string.IsNullOrEmpty(auctionTime) &&
                    DateTime.TryParseExact($"{auctionDate} {auctionTime}", "dd/MM/yyyy HH:mm", null,
                                           System.Globalization.DateTimeStyles.None, out var startDateTime))
                {
                    var currentTime = DateTime.Now;

                    if (startDateTime > currentTime)
                    {
                        var timeRemaining = startDateTime - currentTime;
                        formattedTimeRemaining = FormatTimeSpan(timeRemaining);
                    }
                }

                var auction = new AutionDTO()
                {
                    ListAuctionID = result.ID,
                    owner = result.User.AccountId,
                    Image = result.Image,
                    moneyDeposit = result.MoneyDeposit,
                    NameAuction = result.NameAuction,
                    Description = result.Description,
                    StartingPrice = result.StartingPrice,
                    StatusAuction = result.StatusAuction,
                    StartDay = result.StartDay,
                    StartTime = result.StartTime,
                    EndDay = result.EndDay,
                    EndTime = result.EndTime,
                    NumberofAuctionRounds = result.NumberofAuctionRounds,
                    TimePerLap = result.TimePerLap,
                    PriceStep = result.PriceStep,
                    PaymentMethod = result.PaymentMethod,
                    FileAuctioneer = result.FileAuctioneer,
                    SignatureImg = result.SignatureImg,
                    TImage = result.TImange,
                    countdowntime = formattedTimeRemaining, // Nếu không có ngày/giờ, giá trị sẽ là ""
                    images = result.images,
                    createDate = result.createDate,
                };

                return auction;
            }
            return null;

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
        /// Lists the auctioneer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> ListAuctioneer(int status, string uid)
        {
            List<ListAuctioneerDTO> auction = null;
            var result = await AuctionDAO.Instance.ListAuctioneer(uid);
            if (status == 0)
            {
                auction = result;
            }
            if (status == 1)
            {
                auction = result.Where(ad =>
                    !string.IsNullOrEmpty(ad.EndDay) &&
                    !string.IsNullOrEmpty(ad.EndTime) &&
                    !string.IsNullOrEmpty(ad.TimePerLap) &&
                    DateTime.TryParseExact(ad.EndDay, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDay) &&
                    TimeSpan.TryParse(ad.EndTime, out TimeSpan endTime) &&
                    TimeSpan.TryParse(ad.TimePerLap, out TimeSpan rollTime) &&
                    endDay.Add(endTime) < DateTime.Now &&
                    endDay.Add(endTime).Add(rollTime) > DateTime.Now
                ).ToList();

            }
            else if (status == 2)
            {
                auction = result.Where(ad =>
                    !string.IsNullOrEmpty(ad.StartDay) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) > DateTime.Today ||
                    (!string.IsNullOrEmpty(ad.StartDay) && !string.IsNullOrEmpty(ad.StartTime) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                    TimeSpan.Parse(ad.StartTime) >= DateTime.Now.TimeOfDay)).ToList();
            }
            else if (status == 3)
            {
                auction = result.Where(ad =>
                    !string.IsNullOrEmpty(ad.EndDay) &&
                    !string.IsNullOrEmpty(ad.EndTime) &&
                    !string.IsNullOrEmpty(ad.TimePerLap) &&
                    DateTime.TryParseExact(ad.EndDay, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDay) &&
                    TimeSpan.TryParse(ad.EndTime, out TimeSpan endTime) &&
                    TimeSpan.TryParse(ad.TimePerLap, out TimeSpan rollTime) && 
                    endDay.Add(endTime).Add(rollTime) < DateTime.Now
                ).ToList();
            }
            return auction; // Return the list of auctioneer DTOs
        }

        /// <summary>
        /// Updates the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="auctionDTO">The auction dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateAuction(string id, UpdateAuctionDTO auctionDTO)
        {
            try
            {
                var Image = "";
                var imageEvidence = "";
                if (auctionDTO.imageAuction != null)
                {
                    Image = await _upload.SaveFileAsync(auctionDTO.imageAuction, "ListAuctioneer", id);
                }
                if (auctionDTO.imageEvidence != null)
                {
                    imageEvidence = await _upload.SaveFileAsync(auctionDTO.imageEvidence, "TImages", id);
                }
                var auction = new UDAuctionDTO
                {
                    auctionID = auctionDTO.auctionID,
                    description = auctionDTO.description,
                    category = auctionDTO.category,
                    startingPrice = auctionDTO.startingPrice,
                    nameAuctionItem = auctionDTO.nameAuctionItem,
                    imageAuction = Image,
                    imageEvidence = imageEvidence,
                };
                await AuctionDAO.Instance.UpdateAuctioneer(auction);
                return new ResponseDTO { IsSucceed = true, Message = "Update Auction successfully" };
            }
            catch
            {
                return new ResponseDTO { IsSucceed = false, Message = "Update Auction fail" };
            }
        }

        /// <summary>
        /// Deletes the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> DeleteAuction(int id)
        {
            try
            {
                await AuctionDAO.Instance.DeleteAuctioneer(id);
                return new ResponseDTO { IsSucceed = true, Message = "Delete Auction successfully" };
            }
            catch
            {
                return new ResponseDTO { IsSucceed = false, Message = "Delete Auction fail" };
            }
        }

        /// <summary>
        /// Auctioneers the fl category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="status">The status.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> AuctioneerFlCategory(int category, int status, string uid)
        {
            var auction = new List<ListAuctioneerDTO>();
            var result = await AuctionDAO.Instance.AuctioneerFlCategory(category, uid);
            if (status == 0)
            {
                auction = await AuctionDAO.Instance.ListAuctioneer(uid);
            }
            if (status == 1)
            {
                auction = result.Where(ad =>
                    !string.IsNullOrEmpty(ad.StartDay) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) > DateTime.Today ||
                    (!string.IsNullOrEmpty(ad.StartDay) && !string.IsNullOrEmpty(ad.StartTime) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                    TimeSpan.Parse(ad.StartTime) > DateTime.Now.TimeOfDay)).ToList();
            }
            else if (status == 2)
            {
                auction = result.Where(ad =>
                    !string.IsNullOrEmpty(ad.StartDay) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) < DateTime.Today ||
                    (!string.IsNullOrEmpty(ad.StartDay) && !string.IsNullOrEmpty(ad.StartTime) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                    TimeSpan.Parse(ad.StartTime) <= DateTime.Now.TimeOfDay) &&
                    !string.IsNullOrEmpty(ad.EndDay) &&
                    (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) > DateTime.Today ||
                    (!string.IsNullOrEmpty(ad.EndTime) &&
                    DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                    TimeSpan.Parse(ad.EndTime) >= DateTime.Now.TimeOfDay))).ToList();
            }
            else if (status == 3)
            {
                auction = result.Where(ad =>
                    !string.IsNullOrEmpty(ad.EndDay) &&
                    DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) < DateTime.Today ||
                    (!string.IsNullOrEmpty(ad.EndDay) && !string.IsNullOrEmpty(ad.EndTime) &&
                    DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                    TimeSpan.Parse(ad.EndTime) < DateTime.Now.TimeOfDay)).ToList();
            }


            return auction; // Trả về danh sách các DTO
        }
        /// <summary>
        /// Searchs the auctioneer.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> SearchAuctioneer(string content, string uid, int categoryId)
        {
            var result = await AuctionDAO.Instance.SearchAuctioneer(content, uid);
            if (categoryId > 0)
            {
                result = result.Where(c => c.CategoryId.Equals(categoryId)).ToList();
            }

            return result; // Trả về danh sách các DTO
        }

        /// <summary>
        /// Listofregisteredbidderses the specified userid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="status">The status.</param>
        /// <param name="statusauction">The statusauction.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> Listofregisteredbidders(string userid, int status, bool? statusauction)
        {
            var auction = new List<ListAuctioneerDTO>();
            var result = await RegistAuctionDAO.Instance.Listofregisteredbidders(userid);
            if (status == 0)
            {
                auction = result;
            }
            if (status == 1)
            {
                auction = result.Where(ad =>
                    !string.IsNullOrEmpty(ad.StartDay) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) > DateTime.Today ||
                    (!string.IsNullOrEmpty(ad.StartDay) && !string.IsNullOrEmpty(ad.StartTime) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                    TimeSpan.Parse(ad.StartTime) > DateTime.Now.TimeOfDay)).ToList();
            }
            else if (status == 2)
            {
                auction = result.Where(ad =>
                    !string.IsNullOrEmpty(ad.StartDay) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) < DateTime.Today ||
                    (!string.IsNullOrEmpty(ad.StartDay) && !string.IsNullOrEmpty(ad.StartTime) &&
                    DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                    TimeSpan.Parse(ad.StartTime) <= DateTime.Now.TimeOfDay) &&
                    !string.IsNullOrEmpty(ad.EndDay) &&
                    (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) > DateTime.Today ||
                    (!string.IsNullOrEmpty(ad.EndTime) &&
                    DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                    TimeSpan.Parse(ad.EndTime) >= DateTime.Now.TimeOfDay))).ToList();
            }
            else if (status == 3)
            {
                auction = result.Where(ad =>
                    !string.IsNullOrEmpty(ad.EndDay) &&
                    DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) < DateTime.Today ||
                    (!string.IsNullOrEmpty(ad.EndDay) && !string.IsNullOrEmpty(ad.EndTime) &&
                    DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                    TimeSpan.Parse(ad.EndTime) < DateTime.Now.TimeOfDay)).ToList();
            }
            return auction; // Return the list of auctioneer DTOs
        }

        /// <summary>
        /// Totals the pay.
        /// </summary>
        /// <param name="acutionId">The acution identifier.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public async Task<InforPayMentDTO> TotalPay(int acutionId, string uid)
        {
            return await RegistAuctionDAO.Instance.TotalPay(acutionId, uid);
        }

        /// <summary>
        /// Checks the pay ment.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> CheckPayMent(Payment payment, int id)
        {
            return await RegistAuctionDAO.Instance.CheckPayMent(payment, id);
        }

        /// <summary>
        /// Sends the mail after paymet.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="uid">The uid.</param>
        /// <returns></returns>
        public SetTimeForBatchDTO sendMailAfterPaymet(int id, string uid)
        {
            return RegistAuctionDAO.Instance.sendMailAfterPaymet(id, uid);
        }

        /// <summary>
        /// Lists the auctioneer by user.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListAuctioneerByUser(string id, int status)
        {
            try
            {
                var result = await AuctionDAO.Instance.ListAuctioneerByUser(id, status);
                return new ResponseDTO { Result = result, IsSucceed = true, Message = "Successfully" };
            }
            catch
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed" };
            }
        }

        public async Task<ResponseDTO> ListAuctioneerRegisterByUser(string id, int status)
        {
            try
            {
                var result = await AuctionDAO.Instance.ListAuctioneerRegisterByUser(id, status);
                return new ResponseDTO { Result = result, IsSucceed = true, Message = "Successfully" };
            }
            catch
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed" };
            }
        }

        public async Task<SetTimeForBatch> GetInforSendMail(int id)
        {
            var result = await AuctionDAO.Instance.GetInforSendMail(id);
            return result;
        }
    }
}
