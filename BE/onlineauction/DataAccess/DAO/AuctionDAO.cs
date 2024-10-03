using BusinessObject.Context;
using BusinessObject.Model;
using DataAccess.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace DataAccess.DAO
{
    /// <summary>
    /// 
    /// </summary>
    public class AuctionDAO
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static AuctionDAO _instance = null;
        /// <summary>
        /// The instance lock
        /// </summary>
        private static readonly object _instanceLock = new object();

        /// <summary>
        /// Prevents a default instance of the <see cref="AuctionDAO"/> class from being created.
        /// </summary>
        private AuctionDAO() { }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static AuctionDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new AuctionDAO();
                    }
                    return _instance;
                }
            }
        }
        /// <summary>
        /// Adds the auction.
        /// </summary>
        /// <param name="listAuctioneer">The list auctioneer.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<bool> AddAuction(ListAuction listAuctioneer)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.ListAuctions.Add(listAuctioneer);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Gets the auctioneer.
        /// </summary>
        /// <param name="accountID">The account identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<int> GetAuctioneer(string accountID)
        {
            int id = 0;
            try
            {
                using (var context = new ConnectDB())
                {
                    // Left join to get all auctioneers, even those without matching details
                    var auctioneer = await (from a in context.ListAuctions
                                            join ad in context.AuctionDetails
                                            on a.ListAuctionID equals ad.ListAuctionID into adGroup
                                            from ad in adGroup.DefaultIfEmpty() // Left join, keep ListAuctions data if no match in AuctionDetails
                                            where a.Creator == accountID && ad == null // ad == null means no match in AuctionDetails
                                            select a).FirstOrDefaultAsync();

                    if (auctioneer != null)
                    {
                        id = auctioneer.ListAuctionID;
                    }
                    return id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Lists the auctioneer.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> ListAuctioneer(int status)
        {
            using (var context = new ConnectDB())
            {
                var auctioneerList = await (from a in context.ListAuctions
                                            join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                            where a.StatusAuction == true
                                            select new ListAuctioneerDTO
                                            {
                                                Id = a.ListAuctionID,
                                                Img = a.Image,
                                                Name = a.NameAuction,
                                                StartDay = ad.StartDay,
                                                StartTime = ad.StartTime,
                                                EndDay = ad.EndDay,
                                                EndTime = ad.EndTime,
                                                PriceStart = a.StartingPrice
                                            }).OrderByDescending(o => o.Id).ToListAsync();


                return FilterAuctioneersByStatus(auctioneerList, status);
            }
        }
        /// <summary>
        /// Searchs the auctioneer.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public async Task<List<ListAuctioneerDTO>> SearchAuctioneer(string content)
        {
            using (var context = new ConnectDB())
            {
                var auctioneerList = await (from a in context.ListAuctions
                                            join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                            where a.StatusAuction == true && a.NameAuction.ToLower().Contains(content.ToLower())
                                            select new ListAuctioneerDTO
                                            {
                                                Id = a.ListAuctionID,
                                                Img = a.Image,
                                                Name = a.NameAuction,
                                                StartDay = ad.StartDay,
                                                StartTime = ad.StartTime,
                                                EndDay = ad.EndDay,
                                                EndTime = ad.EndTime,
                                                PriceStart = a.StartingPrice
                                            }).OrderByDescending(o => o.Id).ToListAsync();

                return auctioneerList;
            }
        }

        /// <summary>
        /// Auctioneers the fl category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<List<ListAuctioneerDTO>> AuctioneerFlCategory(int category, int status)
        {
            try
            {
                // Nếu category == 0, trả về tất cả
                if (category == 0)
                {
                    return await ListAuctioneer(status);
                }

                using (var context = new ConnectDB())
                {
                    var auctioneerList = await (from a in context.ListAuctions
                                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                                where a.StatusAuction == true && ad.CategoryID == category
                                                select new ListAuctioneerDTO
                                                {
                                                    Id = a.ListAuctionID,
                                                    Img = a.Image,
                                                    Name = a.NameAuction,
                                                    StartDay = ad.StartDay,
                                                    StartTime = ad.StartTime,
                                                    EndDay = ad.EndDay,
                                                    EndTime = ad.EndTime,
                                                    PriceStart = a.StartingPrice
                                                })
                                                .OrderByDescending(o => o.Id).ToListAsync();

                    return FilterAuctioneersByStatus(auctioneerList, status);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Filters the auctioneers by status.
        /// </summary>
        /// <param name="auctioneerList">The auctioneer list.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        private List<ListAuctioneerDTO> FilterAuctioneersByStatus(List<ListAuctioneerDTO> auctioneerList, int status)
        {
            var today = DateTime.Today;
            var currentTime = DateTime.Now.TimeOfDay;

            DateTime? ParseDate(string date) =>
                DateTime.TryParseExact(date, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out var parsedDate) ? parsedDate : (DateTime?)null;

            TimeSpan? ParseTime(string time) =>
                TimeSpan.TryParse(time, out var parsedTime) ? parsedTime : (TimeSpan?)null;

            if (status == 1) // Upcoming auctions
            {
                return auctioneerList.Where(a =>
                {
                    var startDate = ParseDate(a.StartDay);
                    var startTime = ParseTime(a.StartTime);
                    return startDate > today || (startDate == today && startTime > currentTime);
                }).ToList();
            }
            else if (status == 2) // Ongoing auctions
            {
                return auctioneerList.Where(a =>
                {
                    var startDate = ParseDate(a.StartDay);
                    var startTime = ParseTime(a.StartTime);
                    var endDate = ParseDate(a.EndDay);
                    var endTime = ParseTime(a.EndTime);
                    return (startDate < today || (startDate == today && startTime <= currentTime)) &&
                           (endDate > today || (endDate == today && endTime >= currentTime));
                }).ToList();
            }
            else if (status == 3) // Past auctions
            {
                return auctioneerList.Where(a =>
                {
                    var endDate = ParseDate(a.EndDay);
                    var endTime = ParseTime(a.EndTime);
                    return endDate < today || (endDate == today && endTime < currentTime);
                }).ToList();
            }

            return auctioneerList;
        }


        /// <summary>
        /// Auctions the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<DAuctioneerDTO> AuctionDetail(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var winner = await (from ad in context.AccountDetails
                                        join ac in context.Accounts on ad.AccountID equals ac.Id
                                        join r in context.RegistAuctioneers on ac.Id equals r.AccountID
                                        join b in context.Bets on r.RAID equals b.RAID
                                        join a in context.ListAuctions on r.ListAuctionID equals a.ListAuctionID
                                        where a.ListAuctionID == id
                                        orderby b.PriceBit descending // Order by PriceBit in descending order
                                        select new
                                        {
                                            AccountDetails = ad,
                                            Account = ac,
                                            Bets = b
                                        }).FirstOrDefaultAsync(); // Take only the first entry with the highest PriceBit


                    var auctioneerList = await (from a in context.ListAuctions
                                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                                join us in context.Accounts on a.Creator equals us.Id
                                                join ct in context.Categorys on ad.CategoryID equals ct.CategoryID
                                                join f in context.FileAttachments on ad.ListAuctionID equals f.ListAuctionID
                                                join i in context.TImages on f.FileAID equals i.FileAID
                                                join ud in context.AccountDetails on us.Id equals ud.AccountID
                                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                                from m in adGroup.DefaultIfEmpty()
                                                where a.StatusAuction == true
                                                select new DAuctioneerDTO
                                                {
                                                    ID = a.ListAuctionID,
                                                    User = new ProfileDTO
                                                    {
                                                        AccountId = us.Id,
                                                        UserName = us.UserName,
                                                        Avatar = ud.Avatar,
                                                        FrontCCCD = ud.FrontCCCD,
                                                        BacksideCCCD = ud.BacksideCCCD,
                                                        Email = us.Email,
                                                        FullName = ud.FullName,
                                                        Phone = ud.Phone,
                                                        City = ud.City,
                                                        Ward = ud.Ward,
                                                        District = ud.District,
                                                        Address = ud.Address,
                                                        Warning = us.Warning,
                                                        Status = us.Status,
                                                        Role = "User"
                                                    },
                                                    Manager = a.Manager == null ? "No management yet" : m.FullName,
                                                    Image = a.Image,
                                                    NameAuction = a.NameAuction,
                                                    Description = a.Description,
                                                    StartDay = ad.StartDay,
                                                    StartTime = ad.StartTime,
                                                    EndDay = ad.EndDay,
                                                    EndTime = ad.EndTime,
                                                    StartingPrice = a.StartingPrice,
                                                    categoryName = ct.NameCategory,
                                                    NumberofAuctionRounds = ad.NumberofAuctionRounds,
                                                    TimePerLap = ad.TimePerLap,
                                                    PriceStep = ad.PriceStep,
                                                    PaymentMethod = ad.PaymentMethod,
                                                    FileAuctioneer = f.FileAuctioneer,
                                                    SignatureImg = f.SignatureImg,
                                                    TImange = new TImage
                                                    {
                                                        TImageId = i.TImageId,
                                                        Imange = $"http://capstoneauctioneer.runasp.net/api/Upload/read?filePath={i.Imange}"
                                                    },
                                                    WinBidder = winner,
                                                    StatusAuction = a.StatusAuction == true ? "Approved" : a.StatusAuction == false ? "Reject" : "Not approved yet",
                                                    MoneyDeposit = a.MoneyDeposit
                                                }).FirstOrDefaultAsync();
                    return auctioneerList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Updates the auctioneer.
        /// </summary>
        /// <param name="auctioneer">The auctioneer.</param>
        /// <exception cref="System.Exception">
        /// Account detail not found.
        /// or
        /// An error occurred while updating the account detail: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task UpdateAuctioneer(ListAuction auctioneer)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var existingAutioneer = await context.ListAuctions
                        .FirstOrDefaultAsync(ad => ad.ListAuctionID == auctioneer.ListAuctionID);

                    if (existingAutioneer == null)
                    {
                        throw new Exception("Account detail not found.");
                    }

                    // Cập nhật các thông tin của tài khoản
                    existingAutioneer.Image = auctioneer.Image;
                    existingAutioneer.NameAuction = auctioneer.NameAuction;
                    existingAutioneer.Description = auctioneer.Description;
                    existingAutioneer.StartingPrice = auctioneer.StartingPrice;

                    // Đánh dấu entity là đã sửa đổi và lưu các thay đổi
                    context.Entry(existingAutioneer).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Ghi log lỗi chi tiết hoặc xử lý theo cách bạn muốn
                throw new Exception($"An error occurred while updating the account detail: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác (không phải DbUpdateException)
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Adds the auction detail.
        /// </summary>
        /// <param name="AuctionDetail">The auction detail.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<bool> AddAuctionDetail(AuctionDetail AuctionDetail)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    context.AuctionDetails.Add(AuctionDetail);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch (DbUpdateException ex)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Deletes the auctioneer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public async Task<bool> DeleteAuctioneer(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var lista = await context.ListAuctions.FirstOrDefaultAsync(a => a.ListAuctionID == id && a.StatusAuction == null);
                    if (lista == null) return false;

                    // Auctioneer details
                    var ad = await context.AuctionDetails.FirstOrDefaultAsync(a => a.ListAuctionID == id);
                    if (ad != null)
                    {
                        // File Attachments
                        var file = await context.FileAttachments.Where(a => a.ListAuctionID == id).ToListAsync();
                        if (file.Any())
                        {
                            foreach (var item in file)
                            {
                                // Images related to FileAttachment
                                var img = await context.TImages.Where(i => i.FileAID == item.FileAID).ToListAsync();
                                if (img.Any())
                                {
                                    context.TImages.RemoveRange(img); // Remove images in bulk
                                }
                                context.FileAttachments.Remove(item); // Remove the file attachment
                            }
                        }
                        context.AuctionDetails.Remove(ad); // Remove AuctionDetail
                    }

                    context.ListAuctions.Remove(lista); // Remove the ListAuctioneer

                    await context.SaveChangesAsync(); // Save all changes once after all deletions
                }
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Accepts the auctioneer for admin.
        /// </summary>
        /// <param name="autioneer">The autioneer.</param>
        /// <param name="idManager">The identifier manager.</param>
        /// <exception cref="System.Exception">
        /// Auctioneer not found.
        /// or
        /// Auctioneer details not found.
        /// or
        /// An error occurred while updating the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task AcceptAuctioneerForAdmin(AcceptAutioneerDTO autioneer, string idManager)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var existingAutioneer = await context.ListAuctions
                        .FirstOrDefaultAsync(ad => ad.ListAuctionID == autioneer.AutioneerID);
                    var existingAutioneerDetail = await context.AuctionDetails
                        .FirstOrDefaultAsync(ad => ad.ListAuctionID == autioneer.AutioneerID);

                    if (existingAutioneer == null)
                    {
                        throw new Exception("Auctioneer not found.");
                    }

                    if (existingAutioneerDetail == null)
                    {
                        throw new Exception("Auctioneer details not found.");
                    }

                    // Update auctioneer status and details
                    existingAutioneer.StatusAuction = autioneer.Status;
                    existingAutioneer.Manager = idManager;
                    existingAutioneerDetail.PriceStep = autioneer.PriceStep == null ? 0 : autioneer.PriceStep;

                    // Mark entities as modified
                    context.Entry(existingAutioneer).State = EntityState.Modified;
                    context.Entry(existingAutioneerDetail).State = EntityState.Modified;

                    // Save changes
                    await context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                // Log or handle the detailed database update error
                throw new Exception($"An error occurred while updating the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Lists your auctioneer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<List<AuctionDetailDTO>> ListYourAuctioneer(string id, int status)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join m in context.AccountDetails on a.Manager equals m.AccountID into adGroup
                                from m in adGroup.DefaultIfEmpty()
                                where a.Creator == id
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = a.Manager == null ? "No management yet" : m.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };

                    // Filter based on status
                    if (status == 0)
                    {
                        return await query.ToListAsync();
                    }
                    if (status == 1)
                    {
                        query = query.Where(a => a.StatusAuction == "Not approved yet");
                    }
                    else if (status == 2)
                    {
                        query = query.Where(a => a.StatusAuction == "Reject");
                    }
                    else if (status == 3)
                    {
                        query = query.Where(a => a.StatusAuction == "Approved");
                    }

                    // Execute and return result
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while retrieving the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lists your autioneer detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while updating the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<AuctionDetailDTO> ListYourAutioneerDetail(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var result = await (from a in context.ListAuctions
                                        join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                        join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                        join m in context.AccountDetails
                                        on a.Manager equals m.AccountID into adGroup
                                        from m in adGroup.DefaultIfEmpty()
                                        where a.ListAuctionID == id
                                        select new AuctionDetailDTO
                                        {
                                            ListAuctionID = a.ListAuctionID,
                                            Category = c.NameCategory,
                                            Name = a.Manager == null ? "No management yet" : m.FullName,
                                            Image = a.Image,
                                            NameAuction = a.NameAuction,
                                            StartingPrice = a.StartingPrice,
                                            StartDay = ad.StartDay,
                                            StartTime = ad.StartTime,
                                            EndDay = ad.EndDay,
                                            EndTime = ad.EndTime,
                                            StatusAuction = a.StatusAuction == null ? "Not approved yet" : a.StatusAuction == false ? "Reject" : "Approved"
                                        }).FirstOrDefaultAsync();

                    return result;
                }
            }
            catch (DbUpdateException ex)
            {
                // Log or handle the detailed database update error
                throw new Exception($"An error occurred while updating the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Auctionrooms the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while updating the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<AuctionRoomDTO> Auctionroom(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Fetch data
                    var auctionData = await (from a in context.ListAuctions
                                             join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                             join r in context.RegistAuctioneers on a.ListAuctionID equals r.ListAuctionID
                                             join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                             where a.ListAuctionID == id
                                             select new
                                             {
                                                 a.ListAuctionID,
                                                 c.NameCategory,
                                                 a.Image,
                                                 a.NameAuction,
                                                 a.StartingPrice,
                                                 ad.StartDay,
                                                 ad.StartTime,
                                                 ad.EndDay,
                                                 ad.EndTime,
                                                 r.AuctionStatus
                                             }).FirstOrDefaultAsync();

                    if (auctionData == null)
                    {
                        return null;
                    }

                    // Parse StartDay, StartTime, EndDay, EndTime outside of LINQ
                    DateTime startDate, endDate;
                    TimeSpan startTime, endTime;

                    var isStartDateValid = DateTime.TryParse(auctionData.StartDay, out startDate);
                    var isStartTimeValid = TimeSpan.TryParse(auctionData.StartTime, out startTime);
                    var isEndDateValid = DateTime.TryParse(auctionData.EndDay, out endDate);
                    var isEndTimeValid = TimeSpan.TryParse(auctionData.EndTime, out endTime);

                    bool statusBet = false; // Default value

                    if (isStartDateValid && isStartTimeValid && isEndDateValid && isEndTimeValid)
                    {
                        // Combine date and time
                        DateTime auctionStartDateTime = startDate.Date + startTime;
                        DateTime auctionEndDateTime = endDate.Date + endTime;
                        DateTime currentDateTime = DateTime.Now;

                        if (currentDateTime >= auctionStartDateTime && currentDateTime <= auctionEndDateTime)
                        {
                            // The auction is currently active
                            statusBet = true;
                        }
                        else
                        {
                            // The auction is not active
                            statusBet = false;
                        }
                    }
                    else
                    {
                        // Handle invalid date/time parsing
                        // You may choose to set statusBet to false or throw an exception
                        statusBet = false;
                    }

                    // Map the properties to AuctionRoomDTO and return
                    return new AuctionRoomDTO
                    {
                        ListAuctionID = auctionData.ListAuctionID,
                        Category = auctionData.NameAuction,
                        Image = auctionData.Image,
                        NameAuction = auctionData.NameAuction,
                        StartingPrice = auctionData.StartingPrice,
                        StartDay = auctionData.StartDay,
                        StartTime = auctionData.StartTime,
                        EndDay = auctionData.EndDay,
                        EndTime = auctionData.EndTime,
                        bidStatus = auctionData.AuctionStatus == null ? "No bids yet" : auctionData.AuctionStatus == true ? "You have successfully bid" : "You have failed to bid",
                        statusBet = statusBet
                    };
                }
            }
            catch (DbUpdateException ex)
            {
                // Log or handle the detailed database update error
                throw new Exception($"An error occurred while updating the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle other unexpected errors
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }



        /// <summary>
        /// Lists your auctioneer admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<List<AuctionDetailDTO>> ListYourAuctioneerAdmin(string id, int status)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join u in context.AccountDetails on a.Creator equals u.AccountID
                                where a.Manager == id || a.Manager == null
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = u.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };

                    // Filter based on status
                    if (status == 1)
                    {
                        query = query.Where(a => a.StatusAuction == "Not approved yet");
                    }
                    else if (status == 2)
                    {
                        query = query.Where(a => a.StatusAuction == "Reject");
                    }
                    else if (status == 3)
                    {
                        query = query.Where(a => a.StatusAuction == "Approved");
                    }

                    // Execute and return result
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while retrieving the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Searchs the auctioneer admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<List<AuctionDetailDTO>> SearchAuctioneerAdmin(string id, string content)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join u in context.AccountDetails on a.Creator equals u.AccountID
                                where (a.Manager == id || a.Manager == null) && a.NameAuction.ToLower().Contains(content.ToLower())
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = u.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while retrieving the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Lists your auctioneer category admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// An error occurred while retrieving the auctioneer: {ex.Message}
        /// or
        /// An unexpected error occurred: {ex.Message}
        /// </exception>
        public async Task<List<AuctionDetailDTO>> ListYourAuctioneerCategoryAdmin(string id, int status, int category)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    // Base query
                    var query = from a in context.ListAuctions
                                join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                join c in context.Categorys on ad.CategoryID equals c.CategoryID
                                join u in context.AccountDetails on a.Creator equals u.AccountID
                                where (a.Manager == id || a.Manager == null) && ad.CategoryID == category
                                select new AuctionDetailDTO
                                {
                                    ListAuctionID = a.ListAuctionID,
                                    Category = c.NameCategory,
                                    Name = u.FullName,
                                    Image = a.Image,
                                    NameAuction = a.NameAuction,
                                    StartingPrice = a.StartingPrice,
                                    StartDay = ad.StartDay,
                                    StartTime = ad.StartTime,
                                    EndDay = ad.EndDay,
                                    EndTime = ad.EndTime,
                                    StatusAuction = a.StatusAuction == null ? "Not approved yet"
                                                : a.StatusAuction == false ? "Reject"
                                                : "Approved"
                                };

                    // Filter based on status
                    if (status == 1)
                    {
                        query = query.Where(a => a.StatusAuction == "Not approved yet");
                    }
                    else if (status == 2)
                    {
                        query = query.Where(a => a.StatusAuction == "Reject");
                    }
                    else if (status == 3)
                    {
                        query = query.Where(a => a.StatusAuction == "Approved");
                    }

                    // Execute and return result
                    return await query.OrderByDescending(o => o.ListAuctionID).ToListAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"An error occurred while retrieving the auctioneer: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        /// <summary>Gets the infor send mail.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.Exception">An unexpected error occurred: {ex.Message}</exception>
        public async Task<SetTimeForBatch> GetInforSendMail(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var query = await (from a in context.ListAuctions
                                       join ad in context.AuctionDetails on a.ListAuctionID equals ad.ListAuctionID
                                       join rd in context.RegistAuctioneers on a.ListAuctionID equals rd.ListAuctionID into rGroup
                                       from r in rGroup.DefaultIfEmpty() // left join
                                       join u in context.Accounts on r.AccountID equals u.Id into userGroup // sử dụng into để tạo nhóm
                                       from u in userGroup.DefaultIfEmpty() // left join
                                       join adm in context.Accounts on a.Manager equals adm.Id
                                       join c in context.Accounts on a.Creator equals c.Id
                                       where a.ListAuctionID == Convert.ToInt32(id)
                                       select new
                                       {
                                           EmailAdmin = adm.Email,
                                           AuctioneerEmail = c.Email,

                                           // Lấy email của người đấu giá cao nhất
                                           BidderEmail = (from b in context.Bets
                                                          join ra in context.RegistAuctioneers on b.RAID equals ra.RAID
                                                          join acc in context.Accounts on ra.AccountID equals acc.Id
                                                          where ra.ListAuctionID == a.ListAuctionID
                                                          orderby b.PriceBit descending
                                                          select acc.Email).FirstOrDefault(),

                                           endTime = ConvertToDateTime(ad.EndDay, ad.EndTime),

                                           // Lấy giá đấu cao nhất
                                           Price = r.RAID != null ? (from b in context.Bets
                                                                     where b.RAID == r.RAID
                                                                     select b.PriceBit).OrderByDescending(x => x).FirstOrDefault() : 0,
                                           Account = u,
                                           Title = a,
                                           Admin = adm,
                                           Auction = c
                                       }).FirstOrDefaultAsync();

                    if (query != null)
                    {
                        return new SetTimeForBatch
                        {
                            EmailAdmin = query.EmailAdmin,
                            AuctioneerEmail = query.AuctioneerEmail,
                            BidderEmail = query.BidderEmail,
                            endTime = query.endTime,
                            Price = query.Price,
                            AccountId = query.Account.Id,
                            Title = query.Title.NameAuction,
                            AccountAdminId = query.Admin.Id,
                            AccountAuctionId = query.Auction.Id,
                        };
                    }

                    return null; // Hoặc throw exception tùy theo logic của bạn
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
        public async Task<List<string>> InformationOfTheLoser(int winnerId, int auctionId)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var losers = await (from a in context.Accounts
                                        join r in context.RegistAuctioneers on a.Id equals r.AccountID
                                        where r.ListAuctionID == auctionId && r.RAID != winnerId
                                        select a.Id)
                                        .ToListAsync();

                    // Chuyển đổi List<string> sang List<int>
                    return losers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }



        /// <summary>Converts to date time.</summary>
        /// <param name="endDay">The end day.</param>
        /// <param name="endTime">The end time.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.FormatException">Định dạng EndDay hoặc EndTime không hợp lệ.</exception>
        private DateTime ConvertToDateTime(string endDay, string endTime)
        {
            string combinedDateTime = $"{endDay} {endTime}";

            if (DateTime.TryParseExact(combinedDateTime, "yyyy-MM-dd HH:mm:ss",
                                        System.Globalization.CultureInfo.InvariantCulture,
                                        System.Globalization.DateTimeStyles.None, out DateTime endDateTime))
            {
                return endDateTime;
            }
            else
            {
                throw new FormatException("Định dạng EndDay hoặc EndTime không hợp lệ.");
            }
        }
        /// <summary>Updates the winning status.</summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="System.Exception">An unexpected error occurred: {ex.Message}</exception>
        public async Task<int> UpdateWinningStatus(int id)
        {
            try
            {
                using (var context = new ConnectDB())
                {
                    var highestBidder = await (from r in context.RegistAuctioneers
                                               join b in context.Bets on r.RAID equals b.RAID
                                               where r.ListAuctionID == id
                                               orderby b.PriceBit descending
                                               select r).FirstOrDefaultAsync();
                    if (highestBidder != null)
                    {
                        var auctioneers = await context.RegistAuctioneers
                                        .Where(r => r.ListAuctionID == id)
                                        .ToListAsync();
                        foreach (var auctioneer in auctioneers)
                        {
                            if (auctioneer.RAID == highestBidder.RAID)
                            {
                                auctioneer.AuctionStatus = true;  // Highest bidder
                            }
                            else
                            {
                                auctioneer.AuctionStatus = false; // Other bidders
                            }
                            context.Entry(auctioneer).State = EntityState.Modified;
                            await context.SaveChangesAsync();
                        }
                    }
                    return highestBidder.RAID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }
    }
}
