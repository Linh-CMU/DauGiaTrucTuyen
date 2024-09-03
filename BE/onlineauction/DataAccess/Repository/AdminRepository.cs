﻿using BusinessObject.Model;
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
    public class AdminRepository : IAdminRepository
    {
        private readonly UserManager<Account> _accountManager;
        private readonly IUploadRepository _upload;
        public AdminRepository(UserManager<Account> accountManager, IUploadRepository upload)
        {
            _accountManager = accountManager;
            _upload = upload;
        }
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
            catch(Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Add Category failed" };
            }
        }

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

        public async Task<List<AuctionnerAdminDTO>> ListAuction(string accountID, int status)
        {
            var auctioneerList = new List<AuctionnerAdminDTO>();
            var result = await AuctionDAO.Instance.ListYourAuctioneerAdmin(accountID, status);
            foreach (var item in result)
            {
                // Determine if we should use Start or End day/time
                var isOngoing = DateTime.ParseExact(item.StartDay, "dd/MM/yyyy", null) <= DateTime.Today &&
                    TimeSpan.Parse(item.StartTime) <= DateTime.Now.TimeOfDay;
                var auctionDate = isOngoing ? item.EndDay : item.StartDay;
                var auctionTime = isOngoing ? item.EndTime : item.StartTime;

                // Parse both the date and time together
                if (DateTime.TryParseExact($"{auctionDate} {auctionTime}", "dd/MM/yyyy HH:mm", null,
                                            System.Globalization.DateTimeStyles.None, out var startDateTime))
                {
                    var currentTime = DateTime.Now;
                    var auctioneer = new AuctionnerAdminDTO(); // Create a new object in each iteration

                    string formattedTimeRemaining = "";
                    if (startDateTime > currentTime)
                    {
                        var timeRemaining = startDateTime - currentTime;
                        formattedTimeRemaining = FormatTimeSpan(timeRemaining);
                    }

                    auctioneer.AuctioneerID = item.ListAuctioneerID;
                    auctioneer.Category = item.Category;
                    auctioneer.Name = item.Name;
                    auctioneer.Image = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={item.Image}";
                    auctioneer.NameAuctioneer = item.NameAuctioneer;
                    auctioneer.StartingPrice = item.StartingPrice;
                    auctioneer.StartDay = item.StartDay;
                    auctioneer.StartTime = item.StartTime;
                    auctioneer.EndDay = item.EndDay;
                    auctioneer.EndTime = item.EndTime;
                    auctioneer.StatusAuction = item.StatusAuction;
                    auctioneer.Time = formattedTimeRemaining;

                    auctioneerList.Add(auctioneer); // Add to the list
                }
            }
            return auctioneerList;
        }
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Days * 24 + timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
        public async Task<ResponseDTO> ListCategory()
        {
            try
            {
                var result = await CategoryDAO.Instance.CategoryAsync();
                return new ResponseDTO { Result = result, IsSucceed = true, Message = "List category successfully" };
            }
            catch(Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "List category failed" };
            }
        }

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
                    return new ResponseDTO {IsSucceed = true, Message = "Update category successfully" };
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
                Avatar = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={accountDetail.Avatar}",
                FrontCCCD = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={accountDetail.FrontCCCD}",
                BacksideCCCD = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={accountDetail.BacksideCCCD}",
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

        public async Task<ResponseDTO> AuctioneerDetail(int id)
        {
            try
            {
                var result = await AuctionDAO.Instance.AuctioneerDetail(id);
                return new ResponseDTO { Result = result, IsSucceed = true, Message = "Show auction detail successfully" };
            }
            catch (Exception ex)
            {
                return new ResponseDTO { IsSucceed = false, Message = "Show auction detail failed" };
            }
        }

        public async Task<List<AuctionnerAdminDTO>> SearchAuctioneerAdmin(string id, string content)
        {
            var auctioneerList = new List<AuctionnerAdminDTO>();
            var result = await AuctionDAO.Instance.SearchAuctioneerAdmin(id, content);
            foreach (var item in result)
            {
                // Determine if we should use Start or End day/time
                var isOngoing = DateTime.ParseExact(item.StartDay, "dd/MM/yyyy", null) <= DateTime.Today &&
                    TimeSpan.Parse(item.StartTime) <= DateTime.Now.TimeOfDay;
                var auctionDate = isOngoing ? item.EndDay : item.StartDay;
                var auctionTime = isOngoing ? item.EndTime : item.StartTime;

                // Parse both the date and time together
                if (DateTime.TryParseExact($"{auctionDate} {auctionTime}", "dd/MM/yyyy HH:mm", null,
                                            System.Globalization.DateTimeStyles.None, out var startDateTime))
                {
                    var currentTime = DateTime.Now;
                    var auctioneer = new AuctionnerAdminDTO(); // Create a new object in each iteration

                    string formattedTimeRemaining = "";
                    if (startDateTime > currentTime)
                    {
                        var timeRemaining = startDateTime - currentTime;
                        formattedTimeRemaining = FormatTimeSpan(timeRemaining);
                    }

                    auctioneer.AuctioneerID = item.ListAuctioneerID;
                    auctioneer.Category = item.Category;
                    auctioneer.Name = item.Name;
                    auctioneer.Image = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={item.Image}";
                    auctioneer.NameAuctioneer = item.NameAuctioneer;
                    auctioneer.StartingPrice = item.StartingPrice;
                    auctioneer.StartDay = item.StartDay;
                    auctioneer.StartTime = item.StartTime;
                    auctioneer.EndDay = item.EndDay;
                    auctioneer.EndTime = item.EndTime;
                    auctioneer.StatusAuction = item.StatusAuction;
                    auctioneer.Time = formattedTimeRemaining;

                    auctioneerList.Add(auctioneer); // Add to the list
                }
            }
            return auctioneerList;
        }

        public async Task<List<AuctionnerAdminDTO>> ListYourAuctioneerCategoryAdmin(string id, int status, int category)
        {
            var auctioneerList = new List<AuctionnerAdminDTO>();
            var result = await AuctionDAO.Instance.ListYourAuctioneerCategoryAdmin(id, status, category);
            foreach (var item in result)
            {
                // Determine if we should use Start or End day/time
                var isOngoing = DateTime.ParseExact(item.StartDay, "dd/MM/yyyy", null) <= DateTime.Today &&
                    TimeSpan.Parse(item.StartTime) <= DateTime.Now.TimeOfDay;
                var auctionDate = isOngoing ? item.EndDay : item.StartDay;
                var auctionTime = isOngoing ? item.EndTime : item.StartTime;

                // Parse both the date and time together
                if (DateTime.TryParseExact($"{auctionDate} {auctionTime}", "dd/MM/yyyy HH:mm", null,
                                            System.Globalization.DateTimeStyles.None, out var startDateTime))
                {
                    var currentTime = DateTime.Now;
                    var auctioneer = new AuctionnerAdminDTO(); // Create a new object in each iteration

                    string formattedTimeRemaining = "";
                    if (startDateTime > currentTime)
                    {
                        var timeRemaining = startDateTime - currentTime;
                        formattedTimeRemaining = FormatTimeSpan(timeRemaining);
                    }

                    auctioneer.AuctioneerID = item.ListAuctioneerID;
                    auctioneer.Category = item.Category;
                    auctioneer.Name = item.Name;
                    auctioneer.Image = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={item.Image}";
                    auctioneer.NameAuctioneer = item.NameAuctioneer;
                    auctioneer.StartingPrice = item.StartingPrice;
                    auctioneer.StartDay = item.StartDay;
                    auctioneer.StartTime = item.StartTime;
                    auctioneer.EndDay = item.EndDay;
                    auctioneer.EndTime = item.EndTime;
                    auctioneer.StatusAuction = item.StatusAuction;
                    auctioneer.Time = formattedTimeRemaining;

                    auctioneerList.Add(auctioneer); // Add to the list
                }
            }
            return auctioneerList;
        }
    }
}
