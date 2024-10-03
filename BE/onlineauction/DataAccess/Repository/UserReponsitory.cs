﻿using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
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
    /// <seealso cref="DataAccess.IRepository.IUserReponsitory" />
    public class UserReponsitory : IUserReponsitory
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
        /// Initializes a new instance of the <see cref="UserReponsitory"/> class.
        /// </summary>
        /// <param name="accountManager">The account manager.</param>
        /// <param name="upload">The upload.</param>
        public UserReponsitory(UserManager<Account> accountManager, IUploadRepository upload)
        {
            _accountManager = accountManager;
            _upload = upload;
        }

        /// <summary>
        /// Auctionrooms the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<AuctionRoomDTO> Auctionroom(int id)
        {
            try
            {
                var result = await AuctionDAO.Instance.Auctionroom(id);
                return result;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Lists your auctioneer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListYourAuctioneer(string id, int status)
        {
            try
            {
                var result = await AuctionDAO.Instance.ListYourAuctioneer(id, status);
                if (result != null)
                {
                    return new ResponseDTO { IsSucceed = true, Result = result, Message = "Successfully" };
                }
                return new ResponseDTO { IsSucceed = true, Message = "IsEmpty" };
            }
            catch
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed" };
            }
            
        }

        /// <summary>
        /// Lists your autioneer detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> ListYourAutioneerDetail(int id)
        {
            try
            {
                var result = await AuctionDAO.Instance.ListYourAutioneerDetail(id);
                if (result != null)
                {
                    return new ResponseDTO { IsSucceed = true, Result = result, Message = "Successfully" };
                }
                return new ResponseDTO { IsSucceed = true, Message = "Not found" };
            }
            catch
            {
                return new ResponseDTO { IsSucceed = false, Message = "Failed" };
            }
        }

        /// <summary>
        /// Places the bid.
        /// </summary>
        /// <param name="userid">The userid.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> PlaceBid(string userid, int id)
        {
            try
            {
                var check = await RegistAuctionDAO.Instance.BetAsync(id);
                var idauction = await RegistAuctionDAO.Instance.SelectId(userid, id);
                if (check != null)
                {
                    var bet = new Bet
                    {
                        PriceBit= check.PriceBit,
                        RAID = idauction
                    };
                    var result = await RegistAuctionDAO.Instance.PlaceBid(bet);
                    return result;
                }
                else
                {
                    var bet = new Bet
                    {
                        RAID = idauction
                    };
                    var result = await RegistAuctionDAO.Instance.PlaceBid(bet);
                    return result;
                }
                
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "failed" };
            }
        }

        /// <summary>
        /// Regiters the auctioneer.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="register">The register.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ResponseDTO> RegiterAuctioneer(string userID,RegisterAuctioneerDTO register)
        {
            var account = await _accountManager.FindByIdAsync(userID);
            if (account == null)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Auctioneer not found" };
            }
            var auctioneer = new ListAuction
            {
                Creator = account.Id,
                Image =  await _upload.SaveFileAsync(register.Image, "ListAuctioneer", userID),
                NameAuction = register.NameAuction,
                Description = register.Description,
                StartingPrice = register.StartingPrice
            };
            try
            {
                var result = await AuctionDAO.Instance.AddAuction(auctioneer);
                if (result)
                {
                    var id = await AuctionDAO.Instance.GetAuctioneer(userID);
                    var detailauctioneer = new AuctionDetail
                    {
                        ListAuctionID = id,
                        CategoryID = register.CategoryID,
                        StartDay = register.StartDay,
                        StartTime = register.StartTime,
                        EndDay = register.EndDay,
                        EndTime = register.EndTime,
                        NumberofAuctionRounds = 1,
                        TimePerLap = "1",
                        PaymentMethod = "bid up"
                    };
                    var resultdetail = await AuctionDAO.Instance.AddAuctionDetail(detailauctioneer);
                    if(resultdetail)
                    {
                        var fileAttach = new FileAttachments
                        {
                            ListAuctionID = id,
                            FileAuctioneer = await _upload.SaveFileAsync(register.file, "FileAttachments", userID),
                            SignatureImg = await _upload.SaveFileAsync(register.signatureImg, "FileAttachments", userID),
                        };
                        var resultfile = await FileAttachmentsDAO.Instance.AddFileAttachment(fileAttach);
                        if (resultfile)
                        {
                            var file = await FileAttachmentsDAO.Instance.GetFileAttachments(id);
                            if(file is not null)
                            {
                                var img = new TImage
                                {
                                    FileAID = file.FileAID,
                                    Imange =  await _upload.SaveFileAsync(register.image, "TImage", userID)
                                };
                                var resultimg = await FileAttachmentsDAO.Instance.AddImage(img);
                                if (resultimg)
                                {
                                    return new ResponseDTO { IsSucceed = true, Message = "Add AddAuction successfully" };
                                }
                                else
                                {
                                    return new ResponseDTO { IsSucceed = false, Message = "Add AddAuction failed" };
                                }
                            }
                            else
                            {
                                return new ResponseDTO { IsSucceed = false, Message = "Add AddAuction failed" };
                            }
                        }
                        else
                        {
                            return new ResponseDTO { IsSucceed = false, Message = "Add AddAuction failed" };
                        }

                    }
                }
                else
                {
                    return new ResponseDTO { IsSucceed = false, Message = "Add AddAuction failed" };
                }
            }
            catch(Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Add Auctioneer failed: " + ex.Message };
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Users the register auction.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="auctionId">The auction identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UserRegisterAuction(string userID, int auctionId)
        {
            var register = new RegistAuction
            {
                AccountID = userID,
                ListAuctionID = auctionId,
                PaymentTerm = null,
                AuctionStatus = null
            };
            var result = await RegistAuctionDAO.Instance.RegisterAuction(register);
            return result;
        }

        /// <summary>
        /// Withdraws the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> Withdraw(int id)
        {
            return await RegistAuctionDAO.Instance.Withdraw(id);
        }
        /// <summary>
        /// Views the bid history.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<List<ViewBidHistoryDTO>> ViewBidHistory(int id)
        {
            var result = await RegistAuctionDAO.Instance.ViewBidHistory(id);
            return result;
        }
    }
}
