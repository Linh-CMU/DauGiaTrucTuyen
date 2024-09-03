using BusinessObject.Model;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AuctioneerRepository : IAuctioneerRepository
    {
        private readonly IUploadRepository _upload;
        private readonly UserManager<Account> _accountManager;
        public AuctioneerRepository(UserManager<Account> accountManager, IUploadRepository upload)
        {
            _accountManager = accountManager;
            _upload = upload;
        }
        public async Task<AutionDTO> AuctionDetail(int id)
        {
            var result = await AuctionDAO.Instance.AuctioneerDetail(id);
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
                    ListAuctioneerID = result.ID,
                    Image = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={result.Image}",
                    NameAuctioneer = result.NameAuctioneer,
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
        private string FormatTimeSpan(TimeSpan timeSpan)
        {
            return $"{timeSpan.Days * 24 + timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
        public async Task<List<WSKAuctionnerDTO>> ListAuctioneer(int status)
        {
            var auctioneerList = new List<WSKAuctionnerDTO>();
            var result = await AuctionDAO.Instance.ListAuctioneer(status);
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
                    var auctioneer = new WSKAuctionnerDTO(); // Create a new object in each iteration

                    string formattedTimeRemaining = "";
                    if (startDateTime > currentTime)
                    {
                        var timeRemaining = startDateTime - currentTime;
                        formattedTimeRemaining = FormatTimeSpan(timeRemaining);
                    }

                    auctioneer.ID = item.Id;
                    auctioneer.Img = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={item.Img}";
                    auctioneer.Name = item.Name;
                    auctioneer.Time = formattedTimeRemaining;
                    auctioneer.PriceStart = item.PriceStart;

                    auctioneerList.Add(auctioneer); // Add to the list
                }
            }
            return auctioneerList; // Return the list of auctioneer DTOs
        }

        public async Task<ResponseDTO> UpdateAuction(string id ,UDAuctionDTO auctionDTO)
        {
            try
            {
                var auction = new ListAuctioneer
                {
                    ListAuctioneerID = auctionDTO.AuctionID,
                    Image = await _upload.SaveFileAsync(auctionDTO.Image, "ListAuctioneer", id),
                    NameAuctioneer = auctionDTO.NameAuctioneer,
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

        public async Task<List<WSKAuctionnerDTO>> AuctioneerFlCategory(int category, int status)
        {
            var auctioneerList = new List<WSKAuctionnerDTO>();
            var result = await AuctionDAO.Instance.AuctioneerFlCategory(category, status);

            foreach (var item in result)
            {
                // Xác định phiên đấu giá đang diễn ra hay chưa bắt đầu
                var isOngoing = DateTime.ParseExact(item.StartDay, "dd/MM/yyyy", null) <= DateTime.Today &&
                                TimeSpan.Parse(item.StartTime) <= DateTime.Now.TimeOfDay;
                var auctionDate = isOngoing ? item.EndDay : item.StartDay;
                var auctionTime = isOngoing ? item.EndTime : item.StartTime;

                // Kết hợp ngày và giờ để tạo ra thời gian đầy đủ
                if (DateTime.TryParseExact($"{auctionDate} {auctionTime}", "dd/MM/yyyy HH:mm", null,
                                            System.Globalization.DateTimeStyles.None, out var auctionDateTime))
                {
                    var currentTime = DateTime.Now;
                    var auctioneer = new WSKAuctionnerDTO(); // Tạo đối tượng mới cho từng item

                    // Xử lý định dạng thời gian còn lại
                    string formattedTimeRemaining = "";
                    if (auctionDateTime > currentTime)
                    {
                        var timeRemaining = auctionDateTime - currentTime;
                        formattedTimeRemaining = FormatTimeSpan(timeRemaining); // Định dạng khoảng thời gian còn lại
                    }

                    // Gán các giá trị cho đối tượng DTO
                    auctioneer.ID = item.Id;
                    auctioneer.Img = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={item.Img}";
                    auctioneer.Name = item.Name;
                    auctioneer.Time = formattedTimeRemaining; // Thời gian còn lại (nếu có)
                    auctioneer.PriceStart = item.PriceStart;

                    auctioneerList.Add(auctioneer); // Thêm vào danh sách kết quả
                }
            }

            return auctioneerList; // Trả về danh sách các DTO
        }
        public async Task<List<WSKAuctionnerDTO>> SearchAuctioneer(string content)
        {
            var auctioneerList = new List<WSKAuctionnerDTO>();
            var result = await AuctionDAO.Instance.SearchAuctioneer(content);

            foreach (var item in result)
            {
                // Xác định phiên đấu giá đang diễn ra hay chưa bắt đầu
                var isOngoing = DateTime.ParseExact(item.StartDay, "dd/MM/yyyy", null) <= DateTime.Today &&
                                TimeSpan.Parse(item.StartTime) <= DateTime.Now.TimeOfDay;
                var auctionDate = isOngoing ? item.EndDay : item.StartDay;
                var auctionTime = isOngoing ? item.EndTime : item.StartTime;

                // Kết hợp ngày và giờ để tạo ra thời gian đầy đủ
                if (DateTime.TryParseExact($"{auctionDate} {auctionTime}", "dd/MM/yyyy HH:mm", null,
                                            System.Globalization.DateTimeStyles.None, out var auctionDateTime))
                {
                    var currentTime = DateTime.Now;
                    var auctioneer = new WSKAuctionnerDTO(); // Tạo đối tượng mới cho từng item

                    // Xử lý định dạng thời gian còn lại
                    string formattedTimeRemaining = "";
                    if (auctionDateTime > currentTime)
                    {
                        var timeRemaining = auctionDateTime - currentTime;
                        formattedTimeRemaining = FormatTimeSpan(timeRemaining); // Định dạng khoảng thời gian còn lại
                    }

                    // Gán các giá trị cho đối tượng DTO
                    auctioneer.ID = item.Id;
                    auctioneer.Img = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={item.Img}";
                    auctioneer.Name = item.Name;
                    auctioneer.Time = formattedTimeRemaining; // Thời gian còn lại (nếu có)
                    auctioneer.PriceStart = item.PriceStart;

                    auctioneerList.Add(auctioneer); // Thêm vào danh sách kết quả
                }
            }

            return auctioneerList; // Trả về danh sách các DTO
        }
    }
}
