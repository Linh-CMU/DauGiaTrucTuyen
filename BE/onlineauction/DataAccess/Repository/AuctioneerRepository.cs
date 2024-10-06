﻿using BusinessObject.Model;
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
        /// Initializes a new instance of the <see cref="AuctioneerRepository"/> class.
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
            var isOngoing = DateTime.ParseExact(result.StartDay, "dd/MM/yyyy", null) <= DateTime.Today &&
                    TimeSpan.Parse(result.StartTime) <= DateTime.Now.TimeOfDay;
            var auctionDate = isOngoing ? result.EndDay : result.StartDay;
            var auctionTime = isOngoing ? result.EndTime : result.StartTime;

            // Parse both the date and time together
            if (DateTime.TryParseExact($"{auctionDate} {auctionTime}", "dd/MM/yyyy HH:mm", null,
                                        System.Globalization.DateTimeStyles.None, out var startDateTime))
            {
                var currentTime = DateTime.Now;

                string formattedTimeRemaining = "";
                if (startDateTime > currentTime)
                {
                    var timeRemaining = startDateTime - currentTime;
                    formattedTimeRemaining = FormatTimeSpan(timeRemaining);
                }
                var auction = new AutionDTO()
                {
                    ListAuctionID = result.ID,
                    Image = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={result.Image}",
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
                    FileAuctioneer = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={result.FileAuctioneer}",
                    SignatureImg = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={result.SignatureImg}",
                    TImage = result.TImange,
                    countdowntime = formattedTimeRemaining
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
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> ListAuctioneer(int status, string uid)
        {
            List<ListAuctioneerDTO> auction = null;
            var result = await AuctionDAO.Instance.ListAuctioneer(uid);
            if (status == 0)
            {
                auction = result;
            }
            else if (status == 1)
            {
                auction = result.Where(ad => DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) > DateTime.Today ||
                             (DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.StartTime) > DateTime.Now.TimeOfDay)).ToList();
            }
            else if (status == 2)
            {
                auction = result.Where(ad => DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) < DateTime.Today ||
                             (DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.StartTime) <= DateTime.Now.TimeOfDay) && (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) > DateTime.Today || (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.EndTime) >= DateTime.Now.TimeOfDay))).ToList();
            }
            else if (status == 3)
            {
                auction = result.Where(ad => DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) < DateTime.Today ||
                             (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.EndTime) < DateTime.Now.TimeOfDay)).ToList();
            }
            return auction; // Return the list of auctioneer DTOs
        }

        /// <summary>
        /// Updates the auction.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="auctionDTO">The auction dto.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateAuction(string id, UDAuctionDTO auctionDTO)
        {
            try
            {
                var auction = new ListAuction
                {
                    ListAuctionID = auctionDTO.AuctionID,
                    Image = await _upload.SaveFileAsync(auctionDTO.Image, "ListAuctioneer", id),
                    NameAuction = auctionDTO.NameAuction,
                    Description = auctionDTO.Description,
                    StartingPrice = auctionDTO.StartingPrice,
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
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> AuctioneerFlCategory(int category, int status, string uid)
        {
            var auction = new List<ListAuctioneerDTO>();
            var result = await AuctionDAO.Instance.AuctioneerFlCategory(category, uid);
            if (status == 0)
            {
                auction = await AuctionDAO.Instance.ListAuctioneer(uid);
            }
            else if (status == 1)
            {
                auction = result.Where(ad => DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) > DateTime.Today ||
                             (DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.StartTime) > DateTime.Now.TimeOfDay)).ToList();
            }
            else if (status == 2)
            {
                auction = result.Where(ad => DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) < DateTime.Today ||
                             (DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.StartTime) <= DateTime.Now.TimeOfDay) && (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) > DateTime.Today || (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.EndTime) >= DateTime.Now.TimeOfDay))).ToList();
            }
            else if (status == 3)
            {
                auction = result.Where(ad => DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) < DateTime.Today ||
                             (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.EndTime) < DateTime.Now.TimeOfDay)).ToList();
            }

            return auction; // Trả về danh sách các DTO
        }
        /// <summary>
        /// Searchs the auctioneer.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> SearchAuctioneer(string content, string uid)
        {
            var result = await AuctionDAO.Instance.SearchAuctioneer(content, uid);

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
            var result = await RegistAuctionDAO.Instance.Listofregisteredbidders(userid, statusauction);
            if (status == 0)
            {
                auction = result;
            }
            else if (status == 1)
            {
                auction = result.Where(ad => DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) > DateTime.Today ||
                             (DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.StartTime) > DateTime.Now.TimeOfDay)).ToList();
            }
            else if (status == 2)
            {
                auction = result.Where(ad => DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) < DateTime.Today ||
                             (DateTime.ParseExact(ad.StartDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.StartTime) <= DateTime.Now.TimeOfDay) && (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) > DateTime.Today || (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.EndTime) >= DateTime.Now.TimeOfDay))).ToList();
            }
            else if (status == 3)
            {
                auction = result.Where(ad => DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) < DateTime.Today ||
                             (DateTime.ParseExact(ad.EndDay, "dd/MM/yyyy", null) == DateTime.Today &&
                             TimeSpan.Parse(ad.EndTime) < DateTime.Now.TimeOfDay)).ToList();
            }
            return auction; // Return the list of auctioneer DTOs
        }

        /// <summary>
        /// Totals the pay.
        /// </summary>
        /// <param name="acutionId">The acution identifier.</param>
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

        public SetTimeForBatchDTO sendMailAfterPaymet(int id, string uid)
        {
            return RegistAuctionDAO.Instance.sendMailAfterPaymet(id, uid);
        }
    }
}
